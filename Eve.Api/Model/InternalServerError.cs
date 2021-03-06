/* 
 * EVE Swagger Interface
 *
 * An OpenAPI for EVE Online
 *
 * OpenAPI spec version: 1.2.9
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using SwaggerDateConverter = Eve.Api.Client.SwaggerDateConverter;

namespace Eve.Api.Model
{
    /// <summary>
    /// Internal server error model
    /// </summary>
    [DataContract]
    public partial class InternalServerError :  IEquatable<InternalServerError>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InternalServerError" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected InternalServerError() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="InternalServerError" /> class.
        /// </summary>
        /// <param name="Error">Internal server error message (required).</param>
        public InternalServerError(string Error = default(string))
        {
            // to ensure "Error" is required (not null)
            if (Error == null)
            {
                throw new InvalidDataException("Error is a required property for InternalServerError and cannot be null");
            }
            else
            {
                this.Error = Error;
            }
        }
        
        /// <summary>
        /// Internal server error message
        /// </summary>
        /// <value>Internal server error message</value>
        [DataMember(Name="error", EmitDefaultValue=false)]
        public string Error { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class InternalServerError {\n");
            sb.Append("  Error: ").Append(Error).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
  
        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            return this.Equals(input as InternalServerError);
        }

        /// <summary>
        /// Returns true if InternalServerError instances are equal
        /// </summary>
        /// <param name="input">Instance of InternalServerError to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(InternalServerError input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Error == input.Error ||
                    (this.Error != null &&
                    this.Error.Equals(input.Error))
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 41;
                if (this.Error != null)
                    hashCode = hashCode * 59 + this.Error.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }

}
