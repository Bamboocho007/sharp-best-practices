namespace Authentication_clone.DTOs
{
    public class ResponseData<T>
    {
        public T? Data { get; set; }
        public ResponseMetadata ResponseMetadata { get; set; }

        public ResponseData(T? data)
        {
            Data = data;
            ResponseMetadata = new ResponseMetadata(null, new List<ErrorsDescription>());
        }

        public ResponseData(string errorMessage)
        {
            Data = default;
            ResponseMetadata = new ResponseMetadata(errorMessage, new List<ErrorsDescription>());
        }

        public ResponseData(string errorMessage, List<ErrorsDescription> errors)
        {
            Data = default;
            ResponseMetadata = new ResponseMetadata(errorMessage, errors);
        }
    }

    public class ResponseMetadata {
        public string? Message { get; set; } = null;
        public List<ErrorsDescription> Errors { get; set; }

        public ResponseMetadata(string? message, List<ErrorsDescription> errors)
        {
            Message = message;
            Errors = errors;
        }
    }
}
