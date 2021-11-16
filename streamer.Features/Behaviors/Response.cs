using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace streamer.Features.Behaviors
{
    public class Response
    {
        public Guid? Id { get; set; }
        public bool IsValid { get; set; } = true;

        private readonly IList<string> _messages = new List<string>();

        public IEnumerable<string> Errors { get; }
        public object Result { get; }

        public Response() => Errors = new ReadOnlyCollection<string>(_messages);

        public Response(object result) : this() => Result = result;

        public Response AddError(string message)
        {
            _messages.Add(message);
            IsValid = false;
            return this;
        }
        public static Response AsOkResult(Guid id)
        {
            var result = new Response();
            result.IsValid = true;
            result.Id = id;
            return result;
        }
        public static Response AsErrorResult(string errorMessag)
        {
            var result = new Response();
            result.IsValid = false;
            result.AddError(errorMessag);
            return result;
        }
    }

}
