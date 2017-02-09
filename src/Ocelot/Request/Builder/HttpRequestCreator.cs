﻿using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Ocelot.Responses;

namespace Ocelot.Request.Builder
{
    public sealed class HttpRequestCreator : IRequestCreator
    {
        public async Task<Response<Request>> Build(
            string httpMethod, 
            string downstreamUrl, 
            Stream content, 
            IHeaderDictionary headers,
            IRequestCookieCollection cookies, 
            QueryString queryString, 
            string contentType, 
            RequestId.RequestId requestId)
        {
            var request = await new RequestBuilder()
                .WithHttpMethod(httpMethod)
                .WithDownstreamUrl(downstreamUrl)
                .WithQueryString(queryString)
                .WithContent(content)
                .WithContentType(contentType)
                .WithHeaders(headers)
                .WithRequestId(requestId)
                .WithCookies(cookies)
                .Build();

            return new OkResponse<Request>(request);
        }
    }
}