using System;

namespace gomoku.JobsAPI.Entities
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    class EndpointAttribute : Attribute
    {
        private string endpoint;
        public EndpointAttribute(string endpoint)
        {
            this.endpoint = endpoint;
        }

        public string EndpointAddress
        {
            get { return endpoint; }
        }
    }
}
