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
    /// dogma_attribute object
    /// </summary>
    [DataContract]
    public partial class GetUniverseTypesTypeIdDogmaAttribute :  IEquatable<GetUniverseTypesTypeIdDogmaAttribute>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetUniverseTypesTypeIdDogmaAttribute" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected GetUniverseTypesTypeIdDogmaAttribute() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="GetUniverseTypesTypeIdDogmaAttribute" /> class.
        /// </summary>
        /// <param name="AttributeId">attribute_id integer (required).</param>
        /// <param name="Value">value number (required).</param>
        public GetUniverseTypesTypeIdDogmaAttribute(int? AttributeId = default(int?), float? Value = default(float?))
        {
            // to ensure "AttributeId" is required (not null)
            if (AttributeId == null)
            {
                throw new InvalidDataException("AttributeId is a required property for GetUniverseTypesTypeIdDogmaAttribute and cannot be null");
            }
            else
            {
                this.AttributeId = AttributeId;
            }
            // to ensure "Value" is required (not null)
            if (Value == null)
            {
                throw new InvalidDataException("Value is a required property for GetUniverseTypesTypeIdDogmaAttribute and cannot be null");
            }
            else
            {
                this.Value = Value;
            }
        }
        
        /// <summary>
        /// attribute_id integer
        /// </summary>
        /// <value>attribute_id integer</value>
        [DataMember(Name="attribute_id", EmitDefaultValue=false)]
        public int? AttributeId { get; set; }

        /// <summary>
        /// value number
        /// </summary>
        /// <value>value number</value>
        [DataMember(Name="value", EmitDefaultValue=false)]
        public float? Value { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class GetUniverseTypesTypeIdDogmaAttribute {\n");
            sb.Append("  AttributeId: ").Append(AttributeId).Append("\n");
            sb.Append("  Value: ").Append(Value).Append("\n");
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
            return this.Equals(input as GetUniverseTypesTypeIdDogmaAttribute);
        }

        /// <summary>
        /// Returns true if GetUniverseTypesTypeIdDogmaAttribute instances are equal
        /// </summary>
        /// <param name="input">Instance of GetUniverseTypesTypeIdDogmaAttribute to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(GetUniverseTypesTypeIdDogmaAttribute input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.AttributeId == input.AttributeId ||
                    (this.AttributeId != null &&
                    this.AttributeId.Equals(input.AttributeId))
                ) && 
                (
                    this.Value == input.Value ||
                    (this.Value != null &&
                    this.Value.Equals(input.Value))
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
                if (this.AttributeId != null)
                    hashCode = hashCode * 59 + this.AttributeId.GetHashCode();
                if (this.Value != null)
                    hashCode = hashCode * 59 + this.Value.GetHashCode();
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