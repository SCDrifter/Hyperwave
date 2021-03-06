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
    /// 200 ok object
    /// </summary>
    [DataContract]
    public partial class GetCorporationsCorporationIdStarbasesStarbaseIdOk :  IEquatable<GetCorporationsCorporationIdStarbasesStarbaseIdOk>, IValidatableObject
    {
        /// <summary>
        /// Who can anchor starbase (POS) and its structures
        /// </summary>
        /// <value>Who can anchor starbase (POS) and its structures</value>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum AnchorEnum
        {
            
            /// <summary>
            /// Enum Alliancemember for "alliance_member"
            /// </summary>
            [EnumMember(Value = "alliance_member")]
            Alliancemember,
            
            /// <summary>
            /// Enum Configstarbaseequipmentrole for "config_starbase_equipment_role"
            /// </summary>
            [EnumMember(Value = "config_starbase_equipment_role")]
            Configstarbaseequipmentrole,
            
            /// <summary>
            /// Enum Corporationmember for "corporation_member"
            /// </summary>
            [EnumMember(Value = "corporation_member")]
            Corporationmember,
            
            /// <summary>
            /// Enum Starbasefueltechnicianrole for "starbase_fuel_technician_role"
            /// </summary>
            [EnumMember(Value = "starbase_fuel_technician_role")]
            Starbasefueltechnicianrole
        }

        /// <summary>
        /// Who can take fuel blocks out of the starbase (POS)&#39;s fuel bay
        /// </summary>
        /// <value>Who can take fuel blocks out of the starbase (POS)&#39;s fuel bay</value>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum FuelBayTakeEnum
        {
            
            /// <summary>
            /// Enum Alliancemember for "alliance_member"
            /// </summary>
            [EnumMember(Value = "alliance_member")]
            Alliancemember,
            
            /// <summary>
            /// Enum Configstarbaseequipmentrole for "config_starbase_equipment_role"
            /// </summary>
            [EnumMember(Value = "config_starbase_equipment_role")]
            Configstarbaseequipmentrole,
            
            /// <summary>
            /// Enum Corporationmember for "corporation_member"
            /// </summary>
            [EnumMember(Value = "corporation_member")]
            Corporationmember,
            
            /// <summary>
            /// Enum Starbasefueltechnicianrole for "starbase_fuel_technician_role"
            /// </summary>
            [EnumMember(Value = "starbase_fuel_technician_role")]
            Starbasefueltechnicianrole
        }

        /// <summary>
        /// Who can view the starbase (POS)&#39;s fule bay. Characters either need to have required role or belong to the starbase (POS) owner&#39;s corporation or alliance, as described by the enum, all other access settings follows the same scheme
        /// </summary>
        /// <value>Who can view the starbase (POS)&#39;s fule bay. Characters either need to have required role or belong to the starbase (POS) owner&#39;s corporation or alliance, as described by the enum, all other access settings follows the same scheme</value>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum FuelBayViewEnum
        {
            
            /// <summary>
            /// Enum Alliancemember for "alliance_member"
            /// </summary>
            [EnumMember(Value = "alliance_member")]
            Alliancemember,
            
            /// <summary>
            /// Enum Configstarbaseequipmentrole for "config_starbase_equipment_role"
            /// </summary>
            [EnumMember(Value = "config_starbase_equipment_role")]
            Configstarbaseequipmentrole,
            
            /// <summary>
            /// Enum Corporationmember for "corporation_member"
            /// </summary>
            [EnumMember(Value = "corporation_member")]
            Corporationmember,
            
            /// <summary>
            /// Enum Starbasefueltechnicianrole for "starbase_fuel_technician_role"
            /// </summary>
            [EnumMember(Value = "starbase_fuel_technician_role")]
            Starbasefueltechnicianrole
        }

        /// <summary>
        /// Who can offline starbase (POS) and its structures
        /// </summary>
        /// <value>Who can offline starbase (POS) and its structures</value>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum OfflineEnum
        {
            
            /// <summary>
            /// Enum Alliancemember for "alliance_member"
            /// </summary>
            [EnumMember(Value = "alliance_member")]
            Alliancemember,
            
            /// <summary>
            /// Enum Configstarbaseequipmentrole for "config_starbase_equipment_role"
            /// </summary>
            [EnumMember(Value = "config_starbase_equipment_role")]
            Configstarbaseequipmentrole,
            
            /// <summary>
            /// Enum Corporationmember for "corporation_member"
            /// </summary>
            [EnumMember(Value = "corporation_member")]
            Corporationmember,
            
            /// <summary>
            /// Enum Starbasefueltechnicianrole for "starbase_fuel_technician_role"
            /// </summary>
            [EnumMember(Value = "starbase_fuel_technician_role")]
            Starbasefueltechnicianrole
        }

        /// <summary>
        /// Who can online starbase (POS) and its structures
        /// </summary>
        /// <value>Who can online starbase (POS) and its structures</value>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum OnlineEnum
        {
            
            /// <summary>
            /// Enum Alliancemember for "alliance_member"
            /// </summary>
            [EnumMember(Value = "alliance_member")]
            Alliancemember,
            
            /// <summary>
            /// Enum Configstarbaseequipmentrole for "config_starbase_equipment_role"
            /// </summary>
            [EnumMember(Value = "config_starbase_equipment_role")]
            Configstarbaseequipmentrole,
            
            /// <summary>
            /// Enum Corporationmember for "corporation_member"
            /// </summary>
            [EnumMember(Value = "corporation_member")]
            Corporationmember,
            
            /// <summary>
            /// Enum Starbasefueltechnicianrole for "starbase_fuel_technician_role"
            /// </summary>
            [EnumMember(Value = "starbase_fuel_technician_role")]
            Starbasefueltechnicianrole
        }

        /// <summary>
        /// Who can unanchor starbase (POS) and its structures
        /// </summary>
        /// <value>Who can unanchor starbase (POS) and its structures</value>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum UnanchorEnum
        {
            
            /// <summary>
            /// Enum Alliancemember for "alliance_member"
            /// </summary>
            [EnumMember(Value = "alliance_member")]
            Alliancemember,
            
            /// <summary>
            /// Enum Configstarbaseequipmentrole for "config_starbase_equipment_role"
            /// </summary>
            [EnumMember(Value = "config_starbase_equipment_role")]
            Configstarbaseequipmentrole,
            
            /// <summary>
            /// Enum Corporationmember for "corporation_member"
            /// </summary>
            [EnumMember(Value = "corporation_member")]
            Corporationmember,
            
            /// <summary>
            /// Enum Starbasefueltechnicianrole for "starbase_fuel_technician_role"
            /// </summary>
            [EnumMember(Value = "starbase_fuel_technician_role")]
            Starbasefueltechnicianrole
        }

        /// <summary>
        /// Who can anchor starbase (POS) and its structures
        /// </summary>
        /// <value>Who can anchor starbase (POS) and its structures</value>
        [DataMember(Name="anchor", EmitDefaultValue=false)]
        public AnchorEnum? Anchor { get; set; }
        /// <summary>
        /// Who can take fuel blocks out of the starbase (POS)&#39;s fuel bay
        /// </summary>
        /// <value>Who can take fuel blocks out of the starbase (POS)&#39;s fuel bay</value>
        [DataMember(Name="fuel_bay_take", EmitDefaultValue=false)]
        public FuelBayTakeEnum? FuelBayTake { get; set; }
        /// <summary>
        /// Who can view the starbase (POS)&#39;s fule bay. Characters either need to have required role or belong to the starbase (POS) owner&#39;s corporation or alliance, as described by the enum, all other access settings follows the same scheme
        /// </summary>
        /// <value>Who can view the starbase (POS)&#39;s fule bay. Characters either need to have required role or belong to the starbase (POS) owner&#39;s corporation or alliance, as described by the enum, all other access settings follows the same scheme</value>
        [DataMember(Name="fuel_bay_view", EmitDefaultValue=false)]
        public FuelBayViewEnum? FuelBayView { get; set; }
        /// <summary>
        /// Who can offline starbase (POS) and its structures
        /// </summary>
        /// <value>Who can offline starbase (POS) and its structures</value>
        [DataMember(Name="offline", EmitDefaultValue=false)]
        public OfflineEnum? Offline { get; set; }
        /// <summary>
        /// Who can online starbase (POS) and its structures
        /// </summary>
        /// <value>Who can online starbase (POS) and its structures</value>
        [DataMember(Name="online", EmitDefaultValue=false)]
        public OnlineEnum? Online { get; set; }
        /// <summary>
        /// Who can unanchor starbase (POS) and its structures
        /// </summary>
        /// <value>Who can unanchor starbase (POS) and its structures</value>
        [DataMember(Name="unanchor", EmitDefaultValue=false)]
        public UnanchorEnum? Unanchor { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCorporationsCorporationIdStarbasesStarbaseIdOk" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected GetCorporationsCorporationIdStarbasesStarbaseIdOk() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCorporationsCorporationIdStarbasesStarbaseIdOk" /> class.
        /// </summary>
        /// <param name="AllowAllianceMembers">allow_alliance_members boolean (required).</param>
        /// <param name="AllowCorporationMembers">allow_corporation_members boolean (required).</param>
        /// <param name="Anchor">Who can anchor starbase (POS) and its structures (required).</param>
        /// <param name="AttackIfAtWar">attack_if_at_war boolean (required).</param>
        /// <param name="AttackIfOtherSecurityStatusDropping">attack_if_other_security_status_dropping boolean (required).</param>
        /// <param name="AttackSecurityStatusThreshold">Starbase (POS) will attack if target&#39;s security standing is lower than this value.</param>
        /// <param name="AttackStandingThreshold">Starbase (POS) will attack if target&#39;s standing is lower than this value.</param>
        /// <param name="FuelBayTake">Who can take fuel blocks out of the starbase (POS)&#39;s fuel bay (required).</param>
        /// <param name="FuelBayView">Who can view the starbase (POS)&#39;s fule bay. Characters either need to have required role or belong to the starbase (POS) owner&#39;s corporation or alliance, as described by the enum, all other access settings follows the same scheme (required).</param>
        /// <param name="Fuels">Fuel blocks and other things that will be consumed when operating a starbase (POS).</param>
        /// <param name="Offline">Who can offline starbase (POS) and its structures (required).</param>
        /// <param name="Online">Who can online starbase (POS) and its structures (required).</param>
        /// <param name="Unanchor">Who can unanchor starbase (POS) and its structures (required).</param>
        /// <param name="UseAllianceStandings">True if the starbase (POS) is using alliance standings, otherwise using corporation&#39;s (required).</param>
        public GetCorporationsCorporationIdStarbasesStarbaseIdOk(bool? AllowAllianceMembers = default(bool?), bool? AllowCorporationMembers = default(bool?), AnchorEnum? Anchor = default(AnchorEnum?), bool? AttackIfAtWar = default(bool?), bool? AttackIfOtherSecurityStatusDropping = default(bool?), float? AttackSecurityStatusThreshold = default(float?), float? AttackStandingThreshold = default(float?), FuelBayTakeEnum? FuelBayTake = default(FuelBayTakeEnum?), FuelBayViewEnum? FuelBayView = default(FuelBayViewEnum?), List<GetCorporationsCorporationIdStarbasesStarbaseIdFuel> Fuels = default(List<GetCorporationsCorporationIdStarbasesStarbaseIdFuel>), OfflineEnum? Offline = default(OfflineEnum?), OnlineEnum? Online = default(OnlineEnum?), UnanchorEnum? Unanchor = default(UnanchorEnum?), bool? UseAllianceStandings = default(bool?))
        {
            // to ensure "AllowAllianceMembers" is required (not null)
            if (AllowAllianceMembers == null)
            {
                throw new InvalidDataException("AllowAllianceMembers is a required property for GetCorporationsCorporationIdStarbasesStarbaseIdOk and cannot be null");
            }
            else
            {
                this.AllowAllianceMembers = AllowAllianceMembers;
            }
            // to ensure "AllowCorporationMembers" is required (not null)
            if (AllowCorporationMembers == null)
            {
                throw new InvalidDataException("AllowCorporationMembers is a required property for GetCorporationsCorporationIdStarbasesStarbaseIdOk and cannot be null");
            }
            else
            {
                this.AllowCorporationMembers = AllowCorporationMembers;
            }
            // to ensure "Anchor" is required (not null)
            if (Anchor == null)
            {
                throw new InvalidDataException("Anchor is a required property for GetCorporationsCorporationIdStarbasesStarbaseIdOk and cannot be null");
            }
            else
            {
                this.Anchor = Anchor;
            }
            // to ensure "AttackIfAtWar" is required (not null)
            if (AttackIfAtWar == null)
            {
                throw new InvalidDataException("AttackIfAtWar is a required property for GetCorporationsCorporationIdStarbasesStarbaseIdOk and cannot be null");
            }
            else
            {
                this.AttackIfAtWar = AttackIfAtWar;
            }
            // to ensure "AttackIfOtherSecurityStatusDropping" is required (not null)
            if (AttackIfOtherSecurityStatusDropping == null)
            {
                throw new InvalidDataException("AttackIfOtherSecurityStatusDropping is a required property for GetCorporationsCorporationIdStarbasesStarbaseIdOk and cannot be null");
            }
            else
            {
                this.AttackIfOtherSecurityStatusDropping = AttackIfOtherSecurityStatusDropping;
            }
            // to ensure "FuelBayTake" is required (not null)
            if (FuelBayTake == null)
            {
                throw new InvalidDataException("FuelBayTake is a required property for GetCorporationsCorporationIdStarbasesStarbaseIdOk and cannot be null");
            }
            else
            {
                this.FuelBayTake = FuelBayTake;
            }
            // to ensure "FuelBayView" is required (not null)
            if (FuelBayView == null)
            {
                throw new InvalidDataException("FuelBayView is a required property for GetCorporationsCorporationIdStarbasesStarbaseIdOk and cannot be null");
            }
            else
            {
                this.FuelBayView = FuelBayView;
            }
            // to ensure "Offline" is required (not null)
            if (Offline == null)
            {
                throw new InvalidDataException("Offline is a required property for GetCorporationsCorporationIdStarbasesStarbaseIdOk and cannot be null");
            }
            else
            {
                this.Offline = Offline;
            }
            // to ensure "Online" is required (not null)
            if (Online == null)
            {
                throw new InvalidDataException("Online is a required property for GetCorporationsCorporationIdStarbasesStarbaseIdOk and cannot be null");
            }
            else
            {
                this.Online = Online;
            }
            // to ensure "Unanchor" is required (not null)
            if (Unanchor == null)
            {
                throw new InvalidDataException("Unanchor is a required property for GetCorporationsCorporationIdStarbasesStarbaseIdOk and cannot be null");
            }
            else
            {
                this.Unanchor = Unanchor;
            }
            // to ensure "UseAllianceStandings" is required (not null)
            if (UseAllianceStandings == null)
            {
                throw new InvalidDataException("UseAllianceStandings is a required property for GetCorporationsCorporationIdStarbasesStarbaseIdOk and cannot be null");
            }
            else
            {
                this.UseAllianceStandings = UseAllianceStandings;
            }
            this.AttackSecurityStatusThreshold = AttackSecurityStatusThreshold;
            this.AttackStandingThreshold = AttackStandingThreshold;
            this.Fuels = Fuels;
        }
        
        /// <summary>
        /// allow_alliance_members boolean
        /// </summary>
        /// <value>allow_alliance_members boolean</value>
        [DataMember(Name="allow_alliance_members", EmitDefaultValue=false)]
        public bool? AllowAllianceMembers { get; set; }

        /// <summary>
        /// allow_corporation_members boolean
        /// </summary>
        /// <value>allow_corporation_members boolean</value>
        [DataMember(Name="allow_corporation_members", EmitDefaultValue=false)]
        public bool? AllowCorporationMembers { get; set; }


        /// <summary>
        /// attack_if_at_war boolean
        /// </summary>
        /// <value>attack_if_at_war boolean</value>
        [DataMember(Name="attack_if_at_war", EmitDefaultValue=false)]
        public bool? AttackIfAtWar { get; set; }

        /// <summary>
        /// attack_if_other_security_status_dropping boolean
        /// </summary>
        /// <value>attack_if_other_security_status_dropping boolean</value>
        [DataMember(Name="attack_if_other_security_status_dropping", EmitDefaultValue=false)]
        public bool? AttackIfOtherSecurityStatusDropping { get; set; }

        /// <summary>
        /// Starbase (POS) will attack if target&#39;s security standing is lower than this value
        /// </summary>
        /// <value>Starbase (POS) will attack if target&#39;s security standing is lower than this value</value>
        [DataMember(Name="attack_security_status_threshold", EmitDefaultValue=false)]
        public float? AttackSecurityStatusThreshold { get; set; }

        /// <summary>
        /// Starbase (POS) will attack if target&#39;s standing is lower than this value
        /// </summary>
        /// <value>Starbase (POS) will attack if target&#39;s standing is lower than this value</value>
        [DataMember(Name="attack_standing_threshold", EmitDefaultValue=false)]
        public float? AttackStandingThreshold { get; set; }



        /// <summary>
        /// Fuel blocks and other things that will be consumed when operating a starbase (POS)
        /// </summary>
        /// <value>Fuel blocks and other things that will be consumed when operating a starbase (POS)</value>
        [DataMember(Name="fuels", EmitDefaultValue=false)]
        public List<GetCorporationsCorporationIdStarbasesStarbaseIdFuel> Fuels { get; set; }




        /// <summary>
        /// True if the starbase (POS) is using alliance standings, otherwise using corporation&#39;s
        /// </summary>
        /// <value>True if the starbase (POS) is using alliance standings, otherwise using corporation&#39;s</value>
        [DataMember(Name="use_alliance_standings", EmitDefaultValue=false)]
        public bool? UseAllianceStandings { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class GetCorporationsCorporationIdStarbasesStarbaseIdOk {\n");
            sb.Append("  AllowAllianceMembers: ").Append(AllowAllianceMembers).Append("\n");
            sb.Append("  AllowCorporationMembers: ").Append(AllowCorporationMembers).Append("\n");
            sb.Append("  Anchor: ").Append(Anchor).Append("\n");
            sb.Append("  AttackIfAtWar: ").Append(AttackIfAtWar).Append("\n");
            sb.Append("  AttackIfOtherSecurityStatusDropping: ").Append(AttackIfOtherSecurityStatusDropping).Append("\n");
            sb.Append("  AttackSecurityStatusThreshold: ").Append(AttackSecurityStatusThreshold).Append("\n");
            sb.Append("  AttackStandingThreshold: ").Append(AttackStandingThreshold).Append("\n");
            sb.Append("  FuelBayTake: ").Append(FuelBayTake).Append("\n");
            sb.Append("  FuelBayView: ").Append(FuelBayView).Append("\n");
            sb.Append("  Fuels: ").Append(Fuels).Append("\n");
            sb.Append("  Offline: ").Append(Offline).Append("\n");
            sb.Append("  Online: ").Append(Online).Append("\n");
            sb.Append("  Unanchor: ").Append(Unanchor).Append("\n");
            sb.Append("  UseAllianceStandings: ").Append(UseAllianceStandings).Append("\n");
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
            return this.Equals(input as GetCorporationsCorporationIdStarbasesStarbaseIdOk);
        }

        /// <summary>
        /// Returns true if GetCorporationsCorporationIdStarbasesStarbaseIdOk instances are equal
        /// </summary>
        /// <param name="input">Instance of GetCorporationsCorporationIdStarbasesStarbaseIdOk to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(GetCorporationsCorporationIdStarbasesStarbaseIdOk input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.AllowAllianceMembers == input.AllowAllianceMembers ||
                    (this.AllowAllianceMembers != null &&
                    this.AllowAllianceMembers.Equals(input.AllowAllianceMembers))
                ) && 
                (
                    this.AllowCorporationMembers == input.AllowCorporationMembers ||
                    (this.AllowCorporationMembers != null &&
                    this.AllowCorporationMembers.Equals(input.AllowCorporationMembers))
                ) && 
                (
                    this.Anchor == input.Anchor ||
                    (this.Anchor != null &&
                    this.Anchor.Equals(input.Anchor))
                ) && 
                (
                    this.AttackIfAtWar == input.AttackIfAtWar ||
                    (this.AttackIfAtWar != null &&
                    this.AttackIfAtWar.Equals(input.AttackIfAtWar))
                ) && 
                (
                    this.AttackIfOtherSecurityStatusDropping == input.AttackIfOtherSecurityStatusDropping ||
                    (this.AttackIfOtherSecurityStatusDropping != null &&
                    this.AttackIfOtherSecurityStatusDropping.Equals(input.AttackIfOtherSecurityStatusDropping))
                ) && 
                (
                    this.AttackSecurityStatusThreshold == input.AttackSecurityStatusThreshold ||
                    (this.AttackSecurityStatusThreshold != null &&
                    this.AttackSecurityStatusThreshold.Equals(input.AttackSecurityStatusThreshold))
                ) && 
                (
                    this.AttackStandingThreshold == input.AttackStandingThreshold ||
                    (this.AttackStandingThreshold != null &&
                    this.AttackStandingThreshold.Equals(input.AttackStandingThreshold))
                ) && 
                (
                    this.FuelBayTake == input.FuelBayTake ||
                    (this.FuelBayTake != null &&
                    this.FuelBayTake.Equals(input.FuelBayTake))
                ) && 
                (
                    this.FuelBayView == input.FuelBayView ||
                    (this.FuelBayView != null &&
                    this.FuelBayView.Equals(input.FuelBayView))
                ) && 
                (
                    this.Fuels == input.Fuels ||
                    (this.Fuels != null &&
                    this.Fuels.SequenceEqual(input.Fuels))
                ) && 
                (
                    this.Offline == input.Offline ||
                    (this.Offline != null &&
                    this.Offline.Equals(input.Offline))
                ) && 
                (
                    this.Online == input.Online ||
                    (this.Online != null &&
                    this.Online.Equals(input.Online))
                ) && 
                (
                    this.Unanchor == input.Unanchor ||
                    (this.Unanchor != null &&
                    this.Unanchor.Equals(input.Unanchor))
                ) && 
                (
                    this.UseAllianceStandings == input.UseAllianceStandings ||
                    (this.UseAllianceStandings != null &&
                    this.UseAllianceStandings.Equals(input.UseAllianceStandings))
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
                if (this.AllowAllianceMembers != null)
                    hashCode = hashCode * 59 + this.AllowAllianceMembers.GetHashCode();
                if (this.AllowCorporationMembers != null)
                    hashCode = hashCode * 59 + this.AllowCorporationMembers.GetHashCode();
                if (this.Anchor != null)
                    hashCode = hashCode * 59 + this.Anchor.GetHashCode();
                if (this.AttackIfAtWar != null)
                    hashCode = hashCode * 59 + this.AttackIfAtWar.GetHashCode();
                if (this.AttackIfOtherSecurityStatusDropping != null)
                    hashCode = hashCode * 59 + this.AttackIfOtherSecurityStatusDropping.GetHashCode();
                if (this.AttackSecurityStatusThreshold != null)
                    hashCode = hashCode * 59 + this.AttackSecurityStatusThreshold.GetHashCode();
                if (this.AttackStandingThreshold != null)
                    hashCode = hashCode * 59 + this.AttackStandingThreshold.GetHashCode();
                if (this.FuelBayTake != null)
                    hashCode = hashCode * 59 + this.FuelBayTake.GetHashCode();
                if (this.FuelBayView != null)
                    hashCode = hashCode * 59 + this.FuelBayView.GetHashCode();
                if (this.Fuels != null)
                    hashCode = hashCode * 59 + this.Fuels.GetHashCode();
                if (this.Offline != null)
                    hashCode = hashCode * 59 + this.Offline.GetHashCode();
                if (this.Online != null)
                    hashCode = hashCode * 59 + this.Online.GetHashCode();
                if (this.Unanchor != null)
                    hashCode = hashCode * 59 + this.Unanchor.GetHashCode();
                if (this.UseAllianceStandings != null)
                    hashCode = hashCode * 59 + this.UseAllianceStandings.GetHashCode();
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
