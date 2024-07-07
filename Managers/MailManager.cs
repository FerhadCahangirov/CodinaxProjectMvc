using CodinaxProjectMvc.Areas.Auth.Controllers;
using CodinaxProjectMvc.Business.Abstract.InfrastructureServices;
using CodinaxProjectMvc.Constants;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.DataAccess.Models.Identity;
using CodinaxProjectMvc.Managers.Abstract;
using CodinaxProjectMvc.ViewModel.LayoutVm;
using Couchbase.Search;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;

namespace CodinaxProjectMvc.Managers
{
    public class MailManager : IMailManager
    {
        private readonly IMailSender _mailSender;
        private readonly string _domain;
        private const string _area = "Auth";
        private readonly IConfiguration _configuration;

        public MailManager(IMailSender mailSender, IConfiguration configuration)
        {
            _mailSender = mailSender;
            _domain = configuration[ConfigurationStrings.DomainUrl];
            _configuration = configuration;
        }

        private string MailTemplate(string title, string message, string link, string btnString, string footer, string? downwardText = null)
            => $@"
            <!doctype html>
            <html lang=""en"">
              <head>
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"">
                <title>Simple Transactional Email</title>
                <style media=""all"" type=""text/css"">
                /* -------------------------------------
                GLOBAL RESETS
            ------------------------------------- */
    
                body {{
                  font-family: Helvetica, sans-serif;
                  -webkit-font-smoothing: antialiased;
                  font-size: 16px;
                  line-height: 1.3;
                  -ms-text-size-adjust: 100%;
                  -webkit-text-size-adjust: 100%;
                }}
    
                table {{
                  border-collapse: separate;
                  mso-table-lspace: 0pt;
                  mso-table-rspace: 0pt;
                  width: 100%;
                }}
    
                table td {{
                  font-family: Helvetica, sans-serif;
                  font-size: 16px;
                  vertical-align: top;
                }}
                /* -------------------------------------
                BODY & CONTAINER
            ------------------------------------- */
    
                body {{
                  background-color: #f4f5f6;
                  margin: 0;
                  padding: 0;
                }}
    
                .body {{
                  background-color: #f4f5f6;
                  width: 100%;
                }}
    
                .container {{
                  margin: 0 auto !important;
                  max-width: 600px;
                  padding: 0;
                  padding-top: 24px;
                  width: 600px;
                }}
    
                .content {{
                  box-sizing: border-box;
                  display: block;
                  margin: 0 auto;
                  max-width: 600px;
                  padding: 0;
                }}
                /* -------------------------------------
                HEADER, FOOTER, MAIN
            ------------------------------------- */
    
                .main {{
                  background: #ffffff;
                  border: 1px solid #eaebed;
                  border-radius: 16px;
                  width: 100%;
                }}
    
                .wrapper {{
                  box-sizing: border-box;
                  padding: 24px;
                }}
    
                .footer {{
                  clear: both;
                  padding-top: 24px;
                  text-align: center;
                  width: 100%;
                }}
    
                .footer td,
                .footer p,
                .footer span,
                .footer a {{
                  color: #9a9ea6;
                  font-size: 16px;
                  text-align: center;
                }}
                /* -------------------------------------
                TYPOGRAPHY
            ------------------------------------- */
    
                p {{
                  font-family: Helvetica, sans-serif;
                  font-size: 16px;
                  font-weight: normal;
                  margin: 0;
                  margin-bottom: 16px;
                }}
    
                a {{
                  color: #0867ec;
                  text-decoration: underline;
                }}
                /* -------------------------------------
                BUTTONS
            ------------------------------------- */
    
                .btn {{
                  box-sizing: border-box;
                  min-width: 100% !important;
                  width: 100%;
                }}
    
                .btn > tbody > tr > td {{
                  padding-bottom: 16px;
                }}
    
                .btn table {{
                  width: auto;
                }}
    
                .btn table td {{
                  background-color: #ffffff;
                  border-radius: 4px;
                  text-align: center;
                }}
    
                .btn a {{
                  background-color: #ffffff;
                  border: solid 2px #0867ec;
                  border-radius: 4px;
                  box-sizing: border-box;
                  color: #0867ec;
                  cursor: pointer;
                  display: inline-block;
                  font-size: 16px;
                  font-weight: bold;
                  margin: 0;
                  padding: 12px 24px;
                  text-decoration: none;
                  text-transform: capitalize;
                }}
    
                .btn-primary table td {{
                  background-color: #0867ec;
                }}
    
                .btn-primary a {{
                  background-color: #0867ec;
                  border-color: #0867ec;
                  color: #ffffff;
                }}
    
                @media all {{
                  .btn-primary table td:hover {{
                    background-color: #ec0867 !important;
                  }}
                  .btn-primary a:hover {{
                    background-color: #ec0867 !important;
                    border-color: #ec0867 !important;
                  }}
                }}
    
                /* -------------------------------------
                OTHER STYLES THAT MIGHT BE USEFUL
            ------------------------------------- */
    
                .last {{
                  margin-bottom: 0;
                }}
    
                .first {{
                  margin-top: 0;
                }}
    
                .align-center {{
                  text-align: center;
                }}
    
                .align-right {{
                  text-align: right;
                }}
    
                .align-left {{
                  text-align: left;
                }}
    
                .text-link {{
                  color: #0867ec !important;
                  text-decoration: underline !important;
                }}
    
                .clear {{
                  clear: both;
                }}
    
                .mt0 {{
                  margin-top: 0;
                }}
    
                .mb0 {{
                  margin-bottom: 0;
                }}
    
                .preheader {{
                  color: transparent;
                  display: none;
                  height: 0;
                  max-height: 0;
                  max-width: 0;
                  opacity: 0;
                  overflow: hidden;
                  mso-hide: all;
                  visibility: hidden;
                  width: 0;
                }}
    
                .powered-by a {{
                  text-decoration: none;
                }}
    
                /* -------------------------------------
                RESPONSIVE AND MOBILE FRIENDLY STYLES
            ------------------------------------- */
    
                @media only screen and (max-width: 640px) {{
                  .main p,
                  .main td,
                  .main span {{
                    font-size: 16px !important;
                  }}
                  .wrapper {{
                    padding: 8px !important;
                  }}
                  .content {{
                    padding: 0 !important;
                  }}
                  .container {{
                    padding: 0 !important;
                    padding-top: 8px !important;
                    width: 100% !important;
                  }}
                  .main {{
                    border-left-width: 0 !important;
                    border-radius: 0 !important;
                    border-right-width: 0 !important;
                  }}
                  .btn table {{
                    max-width: 100% !important;
                    width: 100% !important;
                  }}
                  .btn a {{
                    font-size: 16px !important;
                    max-width: 100% !important;
                    width: 100% !important;
                  }}
                }}
                /* -------------------------------------
                PRESERVE THESE STYLES IN THE HEAD
            ------------------------------------- */
    
                @media all {{
                  .ExternalClass {{
                    width: 100%;
                  }}
                  .ExternalClass,
                  .ExternalClass p,
                  .ExternalClass span,
                  .ExternalClass font,
                  .ExternalClass td,
                  .ExternalClass div {{
                    line-height: 100%;
                  }}
                  .apple-link a {{
                    color: inherit !important;
                    font-family: inherit !important;
                    font-size: inherit !important;
                    font-weight: inherit !important;
                    line-height: inherit !important;
                    text-decoration: none !important;
                  }}
                  #MessageViewBody a {{
                    color: inherit;
                    text-decoration: none;
                    font-size: inherit;
                    font-family: inherit;
                    font-weight: inherit;
                    line-height: inherit;
                  }}
                }}

                .footer{{
                        margin-top: 20px;
                        margin-bottom: 20px;

                    }}
                </style>
              </head>
              <body>
                <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"" class=""body"">
                  <tr>
                    <td>&nbsp;</td>
                    <td class=""container"">
                      <div class=""content"">

                        <!-- START CENTERED WHITE CONTAINER -->
                        <span class=""preheader"">This is preheader text. Some clients will show this text as a preview.</span>
                        <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"" class=""main"">

                          <!-- START MAIN CONTENT AREA -->
                          <tr>
                            <td class=""wrapper"">
                              <p>{title}</p>
                              <p>{message}</p>
                              <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"" class=""btn btn-primary"">
                                <tbody>
                                  <tr>
                                    <td align=""left"">
                                      <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"">
                                        <tbody>
                                          <tr>
                                            <td> <a href=""{link}"" target=""_blank"">{btnString}</a> </td>
                                          </tr>
                                        </tbody>
                                      </table>
                                    </td>
                                  </tr>
                                </tbody>
                              </table>
                              <p>{downwardText}</p>
                              <p>{footer}</p>
                            </td>
                          </tr>

                          <!-- END MAIN CONTENT AREA -->
                          </table>
<!-- START FOOTER -->
            <div class=""footer"">
              <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"">
                <tr>
                  <td class=""content-block powered-by"">
                    Copyright 2024 © <a href=""https://codinax.com/"">Codinax Academy</a>
                  </td>
                </tr>
              </table>
            </div>

            <!-- END FOOTER -->
            
            <!-- END CENTERED WHITE CONTAINER --></div>
                    </td>
                    <td>&nbsp;</td>
                  </tr>
                </table>
              </body>
            </html>
            ";

        private string SubscribedMailTemplate(string title, string message, string? footer, string unsubscribeLink)
            => $@"
            <!doctype html>
            <html lang=""en"">
              <head>
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"">
                <title>Simple Transactional Email</title>
                <style media=""all"" type=""text/css"">
                /* -------------------------------------
                GLOBAL RESETS
            ------------------------------------- */
    
                body {{
                  font-family: Helvetica, sans-serif;
                  -webkit-font-smoothing: antialiased;
                  font-size: 16px;
                  line-height: 1.3;
                  -ms-text-size-adjust: 100%;
                  -webkit-text-size-adjust: 100%;
                }}
    
                table {{
                  border-collapse: separate;
                  mso-table-lspace: 0pt;
                  mso-table-rspace: 0pt;
                  width: 100%;
                }}
    
                table td {{
                  font-family: Helvetica, sans-serif;
                  font-size: 16px;
                  vertical-align: top;
                }}
                /* -------------------------------------
                BODY & CONTAINER
            ------------------------------------- */
    
                body {{
                  background-color: #f4f5f6;
                  margin: 0;
                  padding: 0;
                }}
    
                .body {{
                  background-color: #f4f5f6;
                  width: 100%;
                }}
    
                .container {{
                  margin: 0 auto !important;
                  max-width: 600px;
                  padding: 0;
                  padding-top: 24px;
                  width: 600px;
                }}
    
                .content {{
                  box-sizing: border-box;
                  display: block;
                  margin: 0 auto;
                  max-width: 600px;
                  padding: 0;
                }}
                /* -------------------------------------
                HEADER, FOOTER, MAIN
            ------------------------------------- */
    
                .main {{
                  background: #ffffff;
                  border: 1px solid #eaebed;
                  border-radius: 16px;
                  width: 100%;
                }}
    
                .wrapper {{
                  box-sizing: border-box;
                  padding: 24px;
                }}
    
                .footer {{
                  clear: both;
                  padding-top: 24px;
                  text-align: center;
                  width: 100%;
                }}
    
                .footer td,
                .footer p,
                .footer span,
                .footer a {{
                  color: #9a9ea6;
                  font-size: 16px;
                  text-align: center;
                }}
                /* -------------------------------------
                TYPOGRAPHY
            ------------------------------------- */
    
                p {{
                  font-family: Helvetica, sans-serif;
                  font-size: 16px;
                  font-weight: normal;
                  margin: 0;
                  margin-bottom: 16px;
                }}
    
                a {{
                  color: #0867ec;
                  text-decoration: underline;
                }}
                /* -------------------------------------
                BUTTONS
            ------------------------------------- */
    
                .btn {{
                  box-sizing: border-box;
                  min-width: 100% !important;
                  width: 100%;
                }}
    
                .btn > tbody > tr > td {{
                  padding-bottom: 16px;
                }}
    
                .btn table {{
                  width: auto;
                }}
    
                .btn table td {{
                  background-color: #ffffff;
                  border-radius: 4px;
                  text-align: center;
                }}
    
                .btn a {{
                  background-color: #ffffff;
                  border: solid 2px #0867ec;
                  border-radius: 4px;
                  box-sizing: border-box;
                  color: #0867ec;
                  cursor: pointer;
                  display: inline-block;
                  font-size: 16px;
                  font-weight: bold;
                  margin: 0;
                  padding: 12px 24px;
                  text-decoration: none;
                  text-transform: capitalize;
                }}
    
                .btn-primary table td {{
                  background-color: #0867ec;
                }}
    
                .btn-primary a {{
                  background-color: #0867ec;
                  border-color: #0867ec;
                  color: #ffffff;
                }}
    
                @media all {{
                  .btn-primary table td:hover {{
                    background-color: #ec0867 !important;
                  }}
                  .btn-primary a:hover {{
                    background-color: #ec0867 !important;
                    border-color: #ec0867 !important;
                  }}
                }}
    
                /* -------------------------------------
                OTHER STYLES THAT MIGHT BE USEFUL
            ------------------------------------- */
    
                .last {{
                  margin-bottom: 0;
                }}
    
                .first {{
                  margin-top: 0;
                }}
    
                .align-center {{
                  text-align: center;
                }}
    
                .align-right {{
                  text-align: right;
                }}
    
                .align-left {{
                  text-align: left;
                }}
    
                .text-link {{
                  color: #0867ec !important;
                  text-decoration: underline !important;
                }}
    
                .clear {{
                  clear: both;
                }}
    
                .mt0 {{
                  margin-top: 0;
                }}
    
                .mb0 {{
                  margin-bottom: 0;
                }}
    
                .preheader {{
                  color: transparent;
                  display: none;
                  height: 0;
                  max-height: 0;
                  max-width: 0;
                  opacity: 0;
                  overflow: hidden;
                  mso-hide: all;
                  visibility: hidden;
                  width: 0;
                }}
    
                .powered-by a {{
                  text-decoration: none;
                }}
    
                /* -------------------------------------
                RESPONSIVE AND MOBILE FRIENDLY STYLES
            ------------------------------------- */
    
                @media only screen and (max-width: 640px) {{
                  .main p,
                  .main td,
                  .main span {{
                    font-size: 16px !important;
                  }}
                  .wrapper {{
                    padding: 8px !important;
                  }}
                  .content {{
                    padding: 0 !important;
                  }}
                  .container {{
                    padding: 0 !important;
                    padding-top: 8px !important;
                    width: 100% !important;
                  }}
                  .main {{
                    border-left-width: 0 !important;
                    border-radius: 0 !important;
                    border-right-width: 0 !important;
                  }}
                  .btn table {{
                    max-width: 100% !important;
                    width: 100% !important;
                  }}
                  .btn a {{
                    font-size: 16px !important;
                    max-width: 100% !important;
                    width: 100% !important;
                  }}
                }}
                /* -------------------------------------
                PRESERVE THESE STYLES IN THE HEAD
            ------------------------------------- */
    
                @media all {{
                  .ExternalClass {{
                    width: 100%;
                  }}
                  .ExternalClass,
                  .ExternalClass p,
                  .ExternalClass span,
                  .ExternalClass font,
                  .ExternalClass td,
                  .ExternalClass div {{
                    line-height: 100%;
                  }}
                  .apple-link a {{
                    color: inherit !important;
                    font-family: inherit !important;
                    font-size: inherit !important;
                    font-weight: inherit !important;
                    line-height: inherit !important;
                    text-decoration: none !important;
                  }}
                  #MessageViewBody a {{
                    color: inherit;
                    text-decoration: none;
                    font-size: inherit;
                    font-family: inherit;
                    font-weight: inherit;
                    line-height: inherit;
                  }}
                }}

                .footer{{
                        margin-top: 20px;
                        margin-bottom: 20px;

                    }}
                </style>
              </head>
              <body>
                <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"" class=""body"">
                  <tr>
                    <td>&nbsp;</td>
                    <td class=""container"">
                      <div class=""content"">

                        <!-- START CENTERED WHITE CONTAINER -->
                        <span class=""preheader"">This is preheader text. Some clients will show this text as a preview.</span>
                        <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"" class=""main"">

                          <!-- START MAIN CONTENT AREA -->
                          <tr>
                            <td class=""wrapper"">
                              <p>{title}</p>
                              <p>{message}</p>
                              
                              <p>{footer}</p>
                            </td>
                          </tr>

                          <!-- END MAIN CONTENT AREA -->
                          </table>
<!-- START FOOTER -->
            <div class=""footer"">
              <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"">
                <tr>
                  <td class=""content-block"">
                    If you wish to unsubscribe, <a href=""{unsubscribeLink}"">click here</a>.
                  </td>
                </tr>
                <tr>
                  <td class=""content-block powered-by"">
                    Copyright 2024 © <a href=""https://codinax.com/"">Codinax Academy</a>
                  </td>
                </tr>
              </table>
            </div>

            <!-- END FOOTER -->
            
            <!-- END CENTERED WHITE CONTAINER --></div>
                    </td>
                    <td>&nbsp;</td>
                  </tr>
                </table>
              </body>
            </html>
            ";

        private string EmptyMailTemplate(string title, string message, string? footer, string? downwardText = null)
            => $@"
            <!doctype html>
            <html lang=""en"">
              <head>
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"">
                <title>Simple Transactional Email</title>
                <style media=""all"" type=""text/css"">
                /* -------------------------------------
                GLOBAL RESETS
            ------------------------------------- */
    
                body {{
                  font-family: Helvetica, sans-serif;
                  -webkit-font-smoothing: antialiased;
                  font-size: 16px;
                  line-height: 1.3;
                  -ms-text-size-adjust: 100%;
                  -webkit-text-size-adjust: 100%;
                }}
    
                table {{
                  border-collapse: separate;
                  mso-table-lspace: 0pt;
                  mso-table-rspace: 0pt;
                  width: 100%;
                }}
    
                table td {{
                  font-family: Helvetica, sans-serif;
                  font-size: 16px;
                  vertical-align: top;
                }}
                /* -------------------------------------
                BODY & CONTAINER
            ------------------------------------- */
    
                body {{
                  background-color: #f4f5f6;
                  margin: 0;
                  padding: 0;
                }}
    
                .body {{
                  background-color: #f4f5f6;
                  width: 100%;
                }}
    
                .container {{
                  margin: 0 auto !important;
                  max-width: 600px;
                  padding: 0;
                  padding-top: 24px;
                  width: 600px;
                }}
    
                .content {{
                  box-sizing: border-box;
                  display: block;
                  margin: 0 auto;
                  max-width: 600px;
                  padding: 0;
                }}
                /* -------------------------------------
                HEADER, FOOTER, MAIN
            ------------------------------------- */
    
                .main {{
                  background: #ffffff;
                  border: 1px solid #eaebed;
                  border-radius: 16px;
                  width: 100%;
                }}
    
                .wrapper {{
                  box-sizing: border-box;
                  padding: 24px;
                }}
    
                .footer {{
                  clear: both;
                  padding-top: 24px;
                  text-align: center;
                  width: 100%;
                }}
    
                .footer td,
                .footer p,
                .footer span,
                .footer a {{
                  color: #9a9ea6;
                  font-size: 16px;
                  text-align: center;
                }}
                /* -------------------------------------
                TYPOGRAPHY
            ------------------------------------- */
    
                p {{
                  font-family: Helvetica, sans-serif;
                  font-size: 16px;
                  font-weight: normal;
                  margin: 0;
                  margin-bottom: 16px;
                }}
    
                a {{
                  color: #0867ec;
                  text-decoration: underline;
                }}
                /* -------------------------------------
                BUTTONS
            ------------------------------------- */
    
                .btn {{
                  box-sizing: border-box;
                  min-width: 100% !important;
                  width: 100%;
                }}
    
                .btn > tbody > tr > td {{
                  padding-bottom: 16px;
                }}
    
                .btn table {{
                  width: auto;
                }}
    
                .btn table td {{
                  background-color: #ffffff;
                  border-radius: 4px;
                  text-align: center;
                }}
    
                .btn a {{
                  background-color: #ffffff;
                  border: solid 2px #0867ec;
                  border-radius: 4px;
                  box-sizing: border-box;
                  color: #0867ec;
                  cursor: pointer;
                  display: inline-block;
                  font-size: 16px;
                  font-weight: bold;
                  margin: 0;
                  padding: 12px 24px;
                  text-decoration: none;
                  text-transform: capitalize;
                }}
    
                .btn-primary table td {{
                  background-color: #0867ec;
                }}
    
                .btn-primary a {{
                  background-color: #0867ec;
                  border-color: #0867ec;
                  color: #ffffff;
                }}
    
                @media all {{
                  .btn-primary table td:hover {{
                    background-color: #ec0867 !important;
                  }}
                  .btn-primary a:hover {{
                    background-color: #ec0867 !important;
                    border-color: #ec0867 !important;
                  }}
                }}
    
                /* -------------------------------------
                OTHER STYLES THAT MIGHT BE USEFUL
            ------------------------------------- */
    
                .last {{
                  margin-bottom: 0;
                }}
    
                .first {{
                  margin-top: 0;
                }}
    
                .align-center {{
                  text-align: center;
                }}
    
                .align-right {{
                  text-align: right;
                }}
    
                .align-left {{
                  text-align: left;
                }}
    
                .text-link {{
                  color: #0867ec !important;
                  text-decoration: underline !important;
                }}
    
                .clear {{
                  clear: both;
                }}
    
                .mt0 {{
                  margin-top: 0;
                }}
    
                .mb0 {{
                  margin-bottom: 0;
                }}
    
                .preheader {{
                  color: transparent;
                  display: none;
                  height: 0;
                  max-height: 0;
                  max-width: 0;
                  opacity: 0;
                  overflow: hidden;
                  mso-hide: all;
                  visibility: hidden;
                  width: 0;
                }}
    
                .powered-by a {{
                  text-decoration: none;
                }}
    
                /* -------------------------------------
                RESPONSIVE AND MOBILE FRIENDLY STYLES
            ------------------------------------- */
    
                @media only screen and (max-width: 640px) {{
                  .main p,
                  .main td,
                  .main span {{
                    font-size: 16px !important;
                  }}
                  .wrapper {{
                    padding: 8px !important;
                  }}
                  .content {{
                    padding: 0 !important;
                  }}
                  .container {{
                    padding: 0 !important;
                    padding-top: 8px !important;
                    width: 100% !important;
                  }}
                  .main {{
                    border-left-width: 0 !important;
                    border-radius: 0 !important;
                    border-right-width: 0 !important;
                  }}
                  .btn table {{
                    max-width: 100% !important;
                    width: 100% !important;
                  }}
                  .btn a {{
                    font-size: 16px !important;
                    max-width: 100% !important;
                    width: 100% !important;
                  }}
                }}
                /* -------------------------------------
                PRESERVE THESE STYLES IN THE HEAD
            ------------------------------------- */
    
                @media all {{
                  .ExternalClass {{
                    width: 100%;
                  }}
                  .ExternalClass,
                  .ExternalClass p,
                  .ExternalClass span,
                  .ExternalClass font,
                  .ExternalClass td,
                  .ExternalClass div {{
                    line-height: 100%;
                  }}
                  .apple-link a {{
                    color: inherit !important;
                    font-family: inherit !important;
                    font-size: inherit !important;
                    font-weight: inherit !important;
                    line-height: inherit !important;
                    text-decoration: none !important;
                  }}
                  #MessageViewBody a {{
                    color: inherit;
                    text-decoration: none;
                    font-size: inherit;
                    font-family: inherit;
                    font-weight: inherit;
                    line-height: inherit;
                  }}
                }}

                .footer{{
                        margin-top: 20px;
                        margin-bottom: 20px;

                    }}
                </style>
              </head>
              <body>
                <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"" class=""body"">
                  <tr>
                    <td>&nbsp;</td>
                    <td class=""container"">
                      <div class=""content"">

                        <!-- START CENTERED WHITE CONTAINER -->
                        <span class=""preheader"">This is preheader text. Some clients will show this text as a preview.</span>
                        <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"" class=""main"">

                          <!-- START MAIN CONTENT AREA -->
                          <tr>
                            <td class=""wrapper"">
                              <p>{title}</p>
                              <p>{message}</p
                              <p>{downwardText}</p>

                              
                              <p>{footer}</p>
                            </td>
                          </tr>

                          <!-- END MAIN CONTENT AREA -->
                          </table>
<!-- START FOOTER -->
            <div class=""footer"">
              <table role=""presentation"" border=""0"" cellpadding=""0"" cellspacing=""0"">
                <tr>
                  <td class=""content-block powered-by"">
                    Copyright 2024 © <a href=""https://codinax.com/"">Codinax Academy</a>
                  </td>
                </tr>
              </table>
            </div>

            <!-- END FOOTER -->
            
            <!-- END CENTERED WHITE CONTAINER --></div>
                    </td>
                    <td>&nbsp;</td>
                  </tr>
                </table>
              </body>
            </html>
            ";

        public async Task SendConfirmationMailAsync<TEntity>(string token, TEntity user) where TEntity : AppUser
        {
            string controller = "ConfirmMail";
            
            string? confirmationLink = $"{_domain}/{_area}/{controller}?token={Uri.EscapeDataString(token)}&email={user.Email}";

            string message = MailTemplate(
                title: $"Hi {user.FirstName} {user.LastName}",
                message: "We just need to verify your email address before data send to the administraion.",
                link: confirmationLink,
                btnString: "Verify",
                footer: "Thanks! – The Codinax Academy Team.");

            await _mailSender.SendEmailAsync(_configuration[ConfigurationStrings.AccountMailAddr], user.Email, _configuration[ConfigurationStrings.AccountMailPwd], "Codinax Applyment Mail Confirmation", message, true);
        }

        public async Task SendPasswordSetupMailAsync<TEntity>(string token, TEntity user) where TEntity : AppUser
        {
            string controller = "PasswordSetup";

            string? confirmationLink = $"{_domain}/{_area}/{controller}?token={Uri.EscapeDataString(token)}&email={user.Email}";

            string message = MailTemplate(
                title: $"Hi {user.FirstName} {user.LastName}",
                message: "Please click this link to redirect to password setup page",
                link: confirmationLink,
                btnString: "Setup your password",
                footer: "Thanks! – The Codinax Academy Team.");

            await _mailSender.SendEmailAsync(_configuration[ConfigurationStrings.AccountMailAddr], user.Email, _configuration[ConfigurationStrings.AccountMailPwd], "Codinax Applyment Mail Confirmation", message, true);
        }

        public async Task SendPasswordResetMailAsync<TEntity>(string token, TEntity user) where TEntity : AppUser
        {
            string controller = "ForgotPassword";
            string action = "Reset";

            string? confirmationLink = $"{_domain}/{_area}/{controller}/{action}?token={Uri.EscapeDataString(token)}&email={user.Email}";

            string message = MailTemplate(
               title: $"Hi {user.FirstName} {user.LastName}",
               message: "Please click this link to redirect to password reset page",
               link: confirmationLink,
               btnString: "Reset password",
               footer: "Thanks! – The Codinax Academy Team.");

            await _mailSender.SendEmailAsync(_configuration[ConfigurationStrings.AccountMailAddr], user.Email, _configuration[ConfigurationStrings.AccountMailPwd], "Codinax Applyment Mail Confirmation", message, true);
        }

        public async Task SendContactMailAsync(ContactVm contactVm)
            => await _mailSender.SendEmailAsync(_configuration[ConfigurationStrings.ContactMailAddr],
                                             _configuration[ConfigurationStrings.ContactMailAddr],
                                             _configuration[ConfigurationStrings.ContactMailPwd],
                                             contactVm.Email,
                                             PrepareContactContent(contactVm));


        public async Task SendSubscribeConfirmationMailAsync(string token, string email)
        {
            const string controller = "Subscribe";

            string? confirmationLink = $"{_domain}/{_area}/{controller}?token={token}&email={email}";

            string message = MailTemplate(
                title: "Dear Subscriber",
                message: "Thank you for signing up for newsletter! To complete the subscription process and start enjoying all the benefits of our service, please confirm your email address by clicking the link below.",
                link: confirmationLink,
                btnString: "Confirm",
                downwardText: "By confirming your email, you'll gain access to exclusive content, updates, and special offers tailored just for you.<br>If you did not sign up for newsletter, please ignore this email. Your email address will not be subscribed until you confirm by clicking the link above.",
                footer: "Best regards! – The Codinax Academy Team."
                );

            string subject = $"Confirm Your Subscription to Newsletter";

            await _mailSender.SendEmailAsync(
                _configuration[ConfigurationStrings.InfoMailAddr],
                email,
                _configuration[ConfigurationStrings.InfoMailPwd],
                subject,
                message, true);
        }

            
        private string PrepareContactContent(ContactVm vm)
         =>
             @$"
                Fullname: {vm.Firstname} {vm.Lastname}
                Email: {vm.Email}
                PhoneNumber: {vm.PhoneNumber}
                Message: 
                {vm.Message}
            ";

        public async Task SendMailToAllSubscribersAsync(string subject, string content, List<Subscriber> subscribers)
        {
            const string controller = "Subscribe";
            const string action = "UnSubscribe";

            foreach (Subscriber subscriber in subscribers)
            {
                string unsLink = $"{_domain}/{_area}/{controller}/{action}?email={subscriber.Email}";

                await _mailSender.SendEmailAsync(
                _configuration[ConfigurationStrings.InfoMailAddr],
                subscriber.Email,
                _configuration[ConfigurationStrings.InfoMailPwd],
                subject,
                SubscribedMailTemplate(subject, content, "Best regards! – The Codinax Academy Team.", unsLink),
                true);
            }
        }
        public async Task SendSubscribedMailAsync(string email)
        {
            await _mailSender.SendEmailAsync(
                _configuration[ConfigurationStrings.InfoMailAddr],
                email,
                _configuration[ConfigurationStrings.InfoMailPwd],
                "Welcome to Our Newsletter!",
                EmptyMailTemplate("We are thrilled to welcome you to our newsletter community!",
                "By subscribing, you’ll receive the latest updates, exclusive content, special offers, and much more directly in your inbox. We're committed to providing you with valuable information that keeps you informed and inspired.",
                downwardText: "Thank you for joining us. We look forward to connecting with you!",
                footer: "Best regards! – The Codinax Academy Team."
                ), true);
        }

        public async Task SendUnsubscribedMailAsync(string email)
        {
            await _mailSender.SendEmailAsync(
                _configuration[ConfigurationStrings.InfoMailAddr],
                email,
                _configuration[ConfigurationStrings.InfoMailPwd],
                "Unsubscription Confirmation",
                EmptyMailTemplate("We are writing to confirm that you have successfully unsubscribed from our newsletter",
                "We’re sorry to see you go! If you have any feedback on how we can improve our content or if there’s anything specific that led to your decision, we’d love to hear from you. Your insights are invaluable to us.",
                downwardText: "Thank you for your past readership. We hope to serve you again soon.",
                footer: "Best regards! – The Codinax Academy Team."
                ), true);
        }
    }
}
