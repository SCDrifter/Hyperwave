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
    /// Top 10 rankings of corporations by victory points from yesterday, last week and in total
    /// </summary>
    [DataContract]
    public partial class GetFwLeaderboardsCorporationsVictoryPoints :  IEquatable<GetFwLeaderboardsCorporationsVictoryPoints>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetFwLeaderboardsCorporationsVictoryPoints" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected GetFwLeaderboardsCorporationsVictoryPoints() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="GetFwLeaderboardsCorporationsVictoryPoints" /> class.
        /// </summary>
        /// <param name="ActiveTotal">Top 10 ranking of corporations active in faction warfare by total victory points. A corporation is considered \&quot;active\&quot; if they have participated in faction warfare in the past 14 days (required).</param>
        /// <param name="LastWeek">Top 10 ranking of corporations by victory points in the past week (required).</param>
        /// <param name="Yesterday">Top 10 ranking of corporations by victory points in the past day (required).</param>
        public GetFwLeaderboardsCorporationsVictoryPoints(List<GetFwLeaderboardsCorporationsActiveTotalActiveTotal1> ActiveTotal = default(List<GetFwLeaderboardsCorporationsActiveTotalActiveTotal1>), List<GetFwLeaderboardsCorporationsLastWeekLastWeek1> LastWeek = default(List<GetFwLeaderboardsCorporationsLastWeekLastWeek1>), List<GetFwLeaderboardsCorporationsYesterdayYesterday1> Yesterday = default(List<GetFwLeaderboardsCorporationsYesterdayYesterday1>))
        {
            // to ensure "ActiveTotal" is required (not null)
            if (ActiveTotal == null)
            {
                throw new InvalidDataException("ActiveTotal is a required property for GetFwLeaderboardsCorporationsVictoryPoints and cannot be null");
            }
            else
            {
                this.ActiveTotal = ActiveTotal;
            }
            // to ensure "LastWeek" is required (not null)
            if (LastWeek == null)
            {
                throw new InvalidDataException("LastWeek is a required property for GetFwLeaderboardsCorporationsVictoryPoints and cannot be null");
            }
            else
            {
                this.LastWeek = LastWeek;
            }
            // to ensure "Yesterday" is required (not null)
            if (Yesterday == null)
            {
                throw new InvalidDataException("Yesterday is a required property for GetFwLeaderboardsCorporationsVictoryPoints and cannot be null");
            }
            else
            {
                this.Yesterday = Yesterday;
            }
        }
        
        /// <summary>
        /// Top 10 ranking of corporations active in faction warfare by total victory points. A corporation is considered \&quot;active\&quot; if they have participated in faction warfare in the past 14 days
        /// </summary>
        /// <value>Top 10 ranking of corporations active in faction warfare by total victory points. A corporation is considered \&quot;active\&quot; if they have participated in faction warfare in the past 14 days</value>
        [DataMember(Name="active_total", EmitDefaultValue=false)]
        public List<GetFwLeaderboardsCorporationsActiveTotalActiveTotal1> ActiveTotal { get; set; }

        /// <summary>
        /// Top 10 ranking of corporations by victory points in the past week
        /// </summary>
        /// <value>Top 10 ranking of corporations by victory points in the past week</value>
        [DataMember(Name="last_week", EmitDefaultValue=false)]
        public List<GetFwLeaderboardsCorporationsLastWeekLastWeek1> LastWeek { get; set; }

        /// <summary>
        /// Top 10 ranking of corporations by victory points in the past day
        /// </summary>
        /// <value>Top 10 ranking of corporations by victory points in the past day</value>
        [DataMember(Name="yesterday", EmitDefaultValue=false)]
        public List<GetFwLeaderboardsCorporationsYesterdayYesterday1> Yesterday { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class GetFwLeaderboardsCorporationsVictoryPoints {\n");
            sb.Append("  ActiveTotal: ").Append(ActiveTotal).Append("\n");
            sb.Append("  LastWeek: ").Append(LastWeek).Append("\n");
            sb.Append("  Yesterday: ").Append(Yesterday).Append("\n");
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
            return this.Equals(input as GetFwLeaderboardsCorporationsVictoryPoints);
        }

        /// <summary>
        /// Returns true if GetFwLeaderboardsCorporationsVictoryPoints instances are equal
        /// </summary>
        /// <param name="input">Instance of GetFwLeaderboardsCorporationsVictoryPoints to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(GetFwLeaderboardsCorporationsVictoryPoints input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.ActiveTotal == input.ActiveTotal ||
                    (this.ActiveTotal != null &&
                    this.ActiveTotal.SequenceEqual(input.ActiveTotal))
                ) && 
                (
                    this.LastWeek == input.LastWeek ||
                    (this.LastWeek != null &&
                    this.LastWeek.SequenceEqual(input.LastWeek))
                ) && 
                (
                    this.Yesterday == input.Yesterday ||
                    (this.Yesterday != null &&
                    this.Yesterday.SequenceEqual(input.Yesterday))
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
                if (this.ActiveTotal != null)
                    hashCode = hashCode * 59 + this.ActiveTotal.GetHashCode();
                if (this.LastWeek != null)
                    hashCode = hashCode * 59 + this.LastWeek.GetHashCode();
                if (this.Yesterday != null)
                    hashCode = hashCode * 59 + this.Yesterday.GetHashCode();
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
