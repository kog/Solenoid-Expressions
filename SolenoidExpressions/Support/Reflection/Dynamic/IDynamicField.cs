
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

// ReSharper disable UnusedMemberInSuper.Global

namespace Solenoid.Expressions.Support.Reflection.Dynamic
{
	/// <summary>
	/// Defines methods that dynamic field class has to implement.
	/// </summary>
	public interface IDynamicField
	{
		/// <summary>
		/// Gets the value of the dynamic field for the specified target object.
		/// </summary>
		/// <param name="target">
		/// Target object to get field value from.
		/// </param>
		/// <returns>
		/// A field value.
		/// </returns>
		object GetValue(object target);

		/// <summary>
		/// Gets the value of the dynamic field for the specified target object.
		/// </summary>
		/// <param name="target">
		/// Target object to set field value on.
		/// </param>
		/// <param name="value">
		/// A new field value.
		/// </param>
		void SetValue(object target, object value);
	}
}