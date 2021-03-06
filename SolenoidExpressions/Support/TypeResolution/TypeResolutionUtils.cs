/*
 * Copyright � 2002-2011 the original author or authors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

// ReSharper disable UnusedMember.Global


using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Solenoid.Expressions.Support.Util;

namespace Solenoid.Expressions.Support.TypeResolution
{
	/// <summary>
	///     Helper methods with regard to type resolution.
	/// </summary>
	/// <remarks>
	///     <p>
	///         Not intended to be used directly by applications.
	///     </p>
	/// </remarks>
	/// <author>Bruno Baia</author>
	public static class TypeResolutionUtils
	{
		private static readonly ITypeResolver _internalTypeResolver
			= new CachedTypeResolver(new GenericTypeResolver());

		/// <summary>
		///     Resolves the supplied type name into a <see cref="System.Type" />
		///     instance.
		/// </summary>
		/// <remarks>
		///     <p>
		///         If you require special <see cref="System.Type" /> resolution, do
		///         <b>not</b> use this method, but rather instantiate
		///         your own <see cref="TypeResolver" />.
		///     </p>
		/// </remarks>
		/// <param name="typeName">
		///     The (possibly partially assembly qualified) name of a
		///     <see cref="System.Type" />.
		/// </param>
		/// <returns>
		///     A resolved <see cref="System.Type" /> instance.
		/// </returns>
		/// <exception cref="System.TypeLoadException">
		///     If the type cannot be resolved.
		/// </exception>
		public static Type ResolveType(string typeName)
		{
			var type = TypeRegistry.ResolveType(typeName);
			if (type == null)
			{
				type = _internalTypeResolver.Resolve(typeName);
			}
			return type;
		}

		/// <summary>
		///     Resolves a string array of interface names to
		///     a <see cref="System.Type" /> array.
		/// </summary>
		/// <param name="interfaceNames">
		///     An array of valid interface names. Each name must include the full
		///     interface and assembly name.
		/// </param>
		/// <returns>An array of interface <see cref="System.Type" />s.</returns>
		/// <exception cref="System.TypeLoadException">
		///     If any of the interfaces can't be loaded.
		/// </exception>
		/// <exception cref="System.ArgumentException">
		///     If any of the <see cref="System.Type" />s specified is not an interface.
		/// </exception>
		/// <exception cref="System.ArgumentNullException">
		///     If <paramref name="interfaceNames" /> (or any of its elements ) is
		///     <see langword="null" />.
		/// </exception>
		public static IList<Type> ResolveInterfaceArray(string[] interfaceNames)
		{
			AssertUtils.ArgumentNotNull(interfaceNames, "interfaceNames");

			var interfaces = new List<Type>();
			for (var i = 0; i < interfaceNames.Length; i++)
			{
				var interfaceName = interfaceNames[i];
				AssertUtils.ArgumentNotNull(interfaceName,
					string.Format(CultureInfo.InvariantCulture, "interfaceNames[{0}]", i));
				var resolvedInterface = ResolveType(interfaceName);
				if (!resolvedInterface.IsInterface)
				{
					throw new ArgumentException(
						string.Format(CultureInfo.InvariantCulture,
							"[{0}] is a class.",
							resolvedInterface.FullName));
				}
				interfaces.Add(resolvedInterface);
				interfaces.AddRange(resolvedInterface.GetInterfaces());
			}
			return interfaces;
		}


		private static readonly Regex _methodMatchRegex = new Regex(
			@"(?<methodName>([\w]+\.)*[\w\*]+)(?<parameters>(\((?<parameterTypes>[\w\.]+(,[\w\.]+)*)*\))?)",
			RegexOptions.Compiled | RegexOptions.ExplicitCapture);

		/// <summary>
		///     Match a method against the given pattern.
		/// </summary>
		/// <param name="pattern">the pattern to match against.</param>
		/// <param name="method">the method to match.</param>
		/// <returns>
		///     <see lang="true" /> if the method matches the given pattern; otherwise <see lang="false" />.
		/// </returns>
		/// <exception cref="System.ArgumentException">
		///     If the supplied <paramref name="pattern" /> is invalid.
		/// </exception>
		public static bool MethodMatch(string pattern, MethodInfo method)
		{
			var m = _methodMatchRegex.Match(pattern);

			if (!m.Success)
			{
				throw new ArgumentException(String.Format("The pattern [{0}] is not well-formed.", pattern));
			}

			// Check method name
			var methodNamePattern = m.Groups["methodName"].Value;
			if (!PatternMatchUtils.SimpleMatch(methodNamePattern, method.Name))
			{
				return false;
			}

			if (m.Groups["parameters"].Value.Length > 0)
			{
				// Check parameter types
				var parameters = m.Groups["parameterTypes"].Value;
				var paramTypes =
					(parameters.Length == 0)
						? new string[0]
						: StringUtils.DelimitedListToStringArray(parameters, ",");
				var paramInfos = method.GetParameters();

				// Verify parameter count
				if (paramTypes.Length != paramInfos.Length)
				{
					return false;
				}

				// Match parameter types
				var result = !paramInfos.Where((t, i) => t.ParameterType != ResolveType(paramTypes[i])).Any();
				return result;
			}

			return true;
		}
	}
}