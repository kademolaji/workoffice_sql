using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace WorkOffice.Contracts.ServicesContracts
{
    public class CreateResponse
    {
        public object Id { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
    }

    public class GetResponse<T>
    {
        public bool Status { get; set; }
        public T Entity { get; set; }
        public string Message { get; set; }

    }

    public class SearchReply<T>
    {
        public long TotalCount { get; set; }
        public IList<T> Result { get; set; } = new List<T>();
        public IList<string> Errors { get; set; }

        public void AddError(string error)
        {
            if (Errors == null)
                Errors = new List<string>();

            Errors.Add(error);
        }
        public static SearchReply<T> Failed(params string[] errors) => new SearchReply<T> { Errors = errors };
    }

    public class SearchCall<T>
    {
        public int PageSize { get; set; }
        public int From { get; set; }
        public string SortOrder { get; set; }
        public string SortField { get; set; }
        public T Parameter { get; set; }
    }

    public class ApiResponse<T>
    {
        public ApiResponse()
        {
            ErrorMessageList = new List<string>();
        }
        public bool IsSuccess { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public T ResponseType { get; set; }
        public List<string> ErrorMessageList { get; set; }

        public void AddError(string error)
        {
            if (ErrorMessageList == null)
                ErrorMessageList = new List<string>();

            ErrorMessageList.Add(error);
        }
    }


    public class DeleteReply
    {
        public bool Status { get; set; }
        public string Message { get; set; }
    }

    public class ReportResponse
    {
        public bool Status { get; set; }
        public string DownloadUrl { get; set; }
        public string Message { get; set; }
    }

    public class SearchParameter
    {
        public string SearchQuery { get; set; }
    }
    public class ProducesResponseStub
    {
    }
}