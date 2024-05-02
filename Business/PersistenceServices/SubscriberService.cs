using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.DataAccess.Abstract.Repositories;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.Managers.Abstract;
using System;
using System.Security.Cryptography;
using System.Text;

namespace CodinaxProjectMvc.Business.PersistenceServices
{
    public class SubscriberService : ISubscriberService
    {
        private readonly IReadRepository<Subscriber> _subscribeReadRepository;
        private readonly IWriteRepository<Subscriber> _subscribeWriteRepository;
        private readonly IMailManager _mailManager;

        public SubscriberService(
            IReadRepository<Subscriber> subscribeReadRepository,
            IWriteRepository<Subscriber> subscribeWriteRepository,
            IMailManager mailManager)
        {
            _subscribeReadRepository = subscribeReadRepository;
            _subscribeWriteRepository = subscribeWriteRepository;
            _mailManager = mailManager;
        }

        public Task<IEnumerable<Subscriber>> ListSubscribersAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SubscribeAsync(string email)
        {
            Subscriber? check_sub = await _subscribeReadRepository.GetSingleAsync(x => x.Email == email);

            if (check_sub != null)
            {
                if (!check_sub.IsEmailConfirmed)
                {
                    string token_sub = Encrypt(check_sub.TokenValidationKey);
                    await _mailManager.SendSubscribeConfirmationMailAsync(token_sub, check_sub.Email);
                }
                return true;
            }

            Subscriber? subscriber = new Subscriber(email, RandomString);

            await _subscribeWriteRepository.AddAsync(subscriber);
            await _subscribeWriteRepository.SaveAsync();

            string token = Encrypt(subscriber.TokenValidationKey);

            await _mailManager.SendSubscribeConfirmationMailAsync(token, subscriber.Email);

            return true;
        }

        public async Task<bool> ConfirmAsync(string email, string token)
        {
            Subscriber? subscriber = await _subscribeReadRepository.GetSingleAsync(x => x.Email == email);

            if (subscriber == null)
                return false;

            string validation_key = Decrypt(token);

            bool isValidated = subscriber.TokenValidationKey == validation_key;

            if (!isValidated)
                return false;

            subscriber.IsEmailConfirmed = true;

            _subscribeWriteRepository.Update(subscriber);
            await _subscribeWriteRepository.SaveAsync();

            return true;
        }

        private readonly byte[] Key = Encoding.UTF8.GetBytes("b14ca5898a4e4133bbce2ea2315a1916");

        public string Encrypt(string validation_token)
        {
            byte[] iv = GenerateIV();

            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = iv;
                aes.Padding = PaddingMode.Zeros;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        byte[] idBytes = Encoding.UTF8.GetBytes(validation_token);
                        cryptoStream.Write(idBytes, 0, idBytes.Length);
                        cryptoStream.FlushFinalBlock();
                    }

                    return Convert.ToBase64String(memoryStream.ToArray());
                }
            }
        }

        private byte[] GenerateIV()
        {
            using (Aes aes = Aes.Create())
            {
                aes.GenerateIV();
                return aes.IV;
            }
        }

        public string Decrypt(string token)
        {
            byte[] buffer = Convert.FromBase64String(token);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = new byte[aes.BlockSize / 8];
                aes.Padding = PaddingMode.Zeros;


                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }



        private string RandomString
        {
            get
            {
                Random random = new Random();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                return new string(Enumerable.Repeat(chars, 10)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
            }
        }

    }
}
