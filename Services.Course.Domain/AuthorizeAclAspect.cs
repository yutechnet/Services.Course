using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading;
using System.Web;
using BpeProducts.Common.Capabilities;
using BpeProducts.Common.Exceptions;
using BpeProducts.Common.NHibernate;
using BpeProducts.Common.NHibernate.Audit;
using BpeProducts.Services.Acl.Client;
using BpeProducts.Services.Course.Domain.Repositories;
using Castle.DynamicProxy;

namespace BpeProducts.Services.Course.Domain
{
	public class AuthByAclAttribute : Attribute
	{
		public string ObjectIdArgument { get; set; }

		public Capability Capability { get; set; }

		public Type ObjectType { get; set; }

		public string OrganizationObject { get; set; }
	}

	public class AuthorizeAclAspect : IInterceptor
	{
		private readonly IAclHttpClient _aclClient;
		private readonly IAuditDataProvider _auditDataProvider;
		private readonly IRepository _repository;
		private readonly ITokenExtractor _tokenExtractor;

		public AuthorizeAclAspect(IRepository repository, IAclHttpClient aclClient, IAuditDataProvider auditDataProvider,
		                          ITokenExtractor tokenExtractor)
		{
			_repository = repository;
			_aclClient = aclClient;
			_auditDataProvider = auditDataProvider;
			_tokenExtractor = tokenExtractor;
		}

		public void Intercept(IInvocation invocation)
		{
			MethodInfo methodInfo = invocation.MethodInvocationTarget ?? invocation.Method;
			IEnumerable<AuthByAclAttribute> authAttrs =
				methodInfo.GetCustomAttributes(typeof (AuthByAclAttribute), true).Cast<AuthByAclAttribute>();
			AuthByAclAttribute authAttr = authAttrs.FirstOrDefault();

			if (authAttr != null)
			{
				ParameterInfo[] parameters = methodInfo.GetParameters();

				if (!parameters.Any())
					throw new AuthorizationException("missing auth info in aspect");


				if (authAttr.OrganizationObject == null &&
				    (authAttr.ObjectType == null || String.IsNullOrEmpty(authAttr.ObjectIdArgument)))
					throw new AuthorizationException("missing auth info in aspect");

				if (Enum.IsDefined(typeof (Capability), authAttr.Capability) == false)
					throw new AuthorizationException("missing auth info in aspect");

				Guid objectId = Guid.Empty;
				if (!String.IsNullOrEmpty(authAttr.ObjectIdArgument))
				{
					int objectIdPosition =
						parameters.Single(x => x.Name == authAttr.ObjectIdArgument && x.ParameterType == typeof (Guid)).Position;
					objectId = (Guid) invocation.GetArgumentValue(objectIdPosition);
				}

				Guid orgId;
				if (String.IsNullOrEmpty(authAttr.OrganizationObject) == false)
				{
					int orgObjectPosition = parameters.Single(x => x.Name == authAttr.OrganizationObject).Position;
					object orgObject = invocation.GetArgumentValue(orgObjectPosition);
					orgId = (Guid) orgObject.GetType().GetProperty("OrganizationId").GetValue(orgObject);
				}
				else
				{
					var orgEntity = (OrganizationEntity) _repository.Get(authAttr.ObjectType.Name, objectId);
					orgId = orgEntity.OrganizationId;
				}

				Guid uid = _auditDataProvider.GetUserGuid();
				bool hasAccess;
				if (objectId != Guid.Empty)
				{
					hasAccess = _aclClient.HasAccess(_tokenExtractor.GetSamlToken(), uid, orgId, objectId, authAttr.Capability);
				}
				else
				{
					hasAccess = _aclClient.HasAccess(_tokenExtractor.GetSamlToken(), uid, orgId, authAttr.Capability);
				}

				if (hasAccess == false)
				{
					throw new AuthorizationException("Unauthorized, please configure permissions",
					                                 new HttpException(401,
					                                                   "please contact your admin for permission to access this resource"));
					//throw new AuthorizationException(HttpStatusCode.Unauthorized.ToString());//TODO:check from browser
				}
			}

			invocation.Proceed();
		}
	}

	public interface ITokenExtractor
	{
		string GetSamlToken();
	}

	public class TokenExtractor : ITokenExtractor
	{
		public string GetSamlToken()
		{
			var claimsPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;

			var context = (BootstrapContext) claimsPrincipal.Identities.First().BootstrapContext;
			string token = context.Token;

			return token;
		}
	}
}