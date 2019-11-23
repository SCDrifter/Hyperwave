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
    /// mail object
    /// </summary>
    [DataContract]
    public partial class PostCharactersCharacterIdMailMail :  IEquatable<PostCharactersCharacterIdMailMail>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostCharactersCharacterIdMailMail" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected PostCharactersCharacterIdMailMail() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="PostCharactersCharacterIdMailMail" /> class.
        /// </summary>
        /// <param name="ApprovedCost">approved_cost integer (default to 0).</param>
        /// <param name="Body">body string (required).</param>
        /// <param name="Recipients">recipients array (required).</param>
        /// <param name="Subject">subject string (required).</param>
        public PostCharactersCharacterIdMailMail(long? ApprovedCost = 0, string Body = default(string), List<PostCharactersCharacterIdMailRecipient> Recipients = default(List<PostCharactersCharacterIdMailRecipient>), string Subject = default(string))
        {
            // to ensure "Body" is required (not null)
            if (Body == null)
            {
                throw new InvalidDataException("Body is a required property for PostCharactersCharacterIdMailMail and cannot be null");
            }
            else
            {
                this.Body = Body;
            }
            // to ensure "Recipients" is required (not null)
            if (Recipients == null)
            {
                throw new InvalidDataException("Recipients is a required property for PostCharactersCharacterIdMailMail and cannot be null");
            }
            else
            {
                this.Recipients = Recipients;
            }
            // to ensure "Subject" is required (not null)
            if (Subject == null)
            {
                throw new InvalidDataException("Subject is a required property for PostCharactersCharacterIdMailMail and cannot be null");
            }
            else
            {
                this.Subject = Subject;
            }
            // use default value if no "ApprovedCost" provided
            if (ApprovedCost == null)
            {
                this.ApprovedCost = 0;
            }
            else
            {
                this.ApprovedCost = ApprovedCost;
            }
        }
        
        /// <summary>
        /// approved_cost integer
        /// </summary>
        /// <value>approved_cost integer</value>
        [DataMember(Name="approved_cost", EmitDefaultValue=false)]
        public long? ApprovedCost { get; set; }

        /// <summary>
        /// body string
        /// </summary>
        /// <value>body string</value>
        [DataMember(Name="body", EmitDefaultValue=false)]
        public string Body { get; set; }

        /// <summary>
        /// recipients array
        /// </summary>
        /// <value>recipients array</value>
        [DataMember(Name="recipients", EmitDefaultValue=false)]
        public List<PostCharactersCharacterIdMailRecipient> Recipients { get; set; }

        /// <summary>
        /// subject string
        /// </summary>
        /// <value>subject string</value>
        [DataMember(Name="subject", EmitDefaultValue=false)]
        public string Subject { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class PostCharactersCharacterIdMailMail {\n");
            sb.Append("  ApprovedCost: ").Append(ApprovedCost).Append("\n");
            sb.Append("  Body: ").Append(Body).Append("\n");
            sb.Append("  Recipients: ").Append(Recipients).Append("\n");
            sb.Append("  Subject: ").Append(Subject).Append("\n");
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
            return this.Equals(input as PostCharactersCharacterIdMailMail);
        }

        /// <summary>
        /// Returns true if PostCharactersCharacterIdMailMail instances are equal
        /// </summary>
        /// <param name="input">Instance of PostCharactersCharacterIdMailMail to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(PostCharactersCharacterIdMailMail input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.ApprovedCost == input.ApprovedCost ||
                    (this.ApprovedCost != null &&
                    this.ApprovedCost.Equals(input.ApprovedCost))
                ) && 
                (
                    this.Body == input.Body ||
                    (this.Body != null &&
                    this.Body.Equals(input.Body))
                ) && 
                (
                    this.Recipients == input.Recipients ||
                    (this.Recipients != null &&
                    this.Recipients.SequenceEqual(input.Recipients))
                ) && 
                (
                    this.Subject == input.Subject ||
                    (this.Subject != null &&
                    this.Subject.Equals(input.Subject))
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
                if (this.ApprovedCost != null)
                    hashCode = hashCode * 59 + this.ApprovedCost.GetHashCode();
                if (this.Body != null)
                    hashCode = hashCode * 59 + this.Body.GetHashCode();
                if (this.Recipients != null)
                    hashCode = hashCode * 59 + this.Recipients.GetHashCode();
                if (this.Subject != null)
                    hashCode = hashCode * 59 + this.Subject.GetHashCode();
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
            // Body (string) maxLength
            if(this.Body != null && this.Body.Length > 10000)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for Body, length must be less than 10000.", new [] { "Body" });
            }

            // Subject (string) maxLength
            if(this.Subject != null && this.Subject.Length > 1000)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for Subject, length must be less than 1000.", new [] { "Subject" });
            }

            yield break;
        }
    }

}
