using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.Enums;
using CodinaxProjectMvc.ViewModel.LectureVm;

namespace CodinaxProjectMvc.Business.Abstract.PersistenceServices
{
    public interface ILectureService
    {
        Task<bool> CreateLectureAsync(LectureCreateVm lectureCreateVm);

        Task<LectureUpdateVm> GetLectureUpdateDataAsync(Guid id);

        Task<bool> UpdateLecturesAsync(LectureUpdateVm lectureUpdateVm);

        Task<bool> AddLectureFileAsync(LectureFileCreateVm lectureFileCreateVm);


    }
}
