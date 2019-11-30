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
    /// contents object
    /// </summary>
    [DataContract]
    public partial class PutCharactersCharacterIdMailMailIdContents :  IEquatable<PutCharactersCharacterIdMailMailIdContents>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PutCharactersCharacterIdMailMailIdContents" /> class.
        /// </summary>
        /// <param name="Labels">Labels to assign to the mail. Pre-existing labels are unassigned..</param>
        /// <param name="Read">Whether the mail is flagged as read.</param>
        public PutCharactersCharacterIdMailMailIdContents(List<int?> Labels = default(List<int?>), bool? Read = default(bool?))
        {
            this.Labels = Labels;
            this.Read = Read;
        }
        
        /// <summary>
        /// Labels to assign to the mail. Pre-existing labels are unassigned.
        /// </summary>
        /// <value>Labels to assign to the mail. Pre-existing labels are unassigned.</value>
        [DataMember(Name="labels", EmitDefaultValue=false)]
        public List<int?> Labels { get; set; }

        /// <summary>
        /// Whether the mail is flagged as read
        /// </summary>
        /// <value>Whether the mail is flagged as read</value>
        [DataMember(Name="read", EmitDefaultValue=false)]
        public bool? Read { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class PutCharactersCharacterIdMailMailIdContents {\n");
            sb.Append("  Labels: ").Append(Labels).Append("\n");
            sb.Append("  Read: ").Append(Read).Append("\n");
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
            return this.Equals(input as PutCharactersCharacterIdMailMailIdContents);
        }

        /// <summary>
        /// Returns true if PutCharactersCharacterIdMailMailIdContents instances are equal
        /// </summary>
        /// <param name="input">Instance of PutCharactersCharacterIdMailMailIdContents to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(PutCharactersCharacterIdMailMailIdContents input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Labels == input.Labels ||
                    (this.Labels != null &&
                    this.Labels.SequenceEqual(input.Labels))
                ) && 
                (
                    this.Read == input.Read ||
                    (this.Read != null &&
                    this.Read.Equals(input.Read))
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
                if (this.Labels != null)
                    hashCode = hashCode * 59 + this.Labels.GetHashCode();
                if (this.Read != null)
                    hashCode = hashCode * 59 + this.Read.GetHashCode();
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