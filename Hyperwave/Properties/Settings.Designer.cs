﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Hyperwave.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.3.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("BeforeMessageLoad")]
        public global::Hyperwave.Config.EmailReadAction MailReadAction {
            get {
                return ((global::Hyperwave.Config.EmailReadAction)(this["MailReadAction"]));
            }
            set {
                this["MailReadAction"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("dd-MMM-yyyy")]
        public string DateFormat {
            get {
                return ((string)(this["DateFormat"]));
            }
            set {
                this["DateFormat"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("h:mm:ss")]
        public string TimeFormat {
            get {
                return ((string)(this["TimeFormat"]));
            }
            set {
                this["TimeFormat"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("60")]
        public decimal MailCheckInterval {
            get {
                return ((decimal)(this["MailCheckInterval"]));
            }
            set {
                this["MailCheckInterval"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1800")]
        public decimal BackgroundMailCheckInterval {
            get {
                return ((decimal)(this["BackgroundMailCheckInterval"]));
            }
            set {
                this["BackgroundMailCheckInterval"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool BackgroundEnabled {
            get {
                return ((bool)(this["BackgroundEnabled"]));
            }
            set {
                this["BackgroundEnabled"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool ShowNotifications {
            get {
                return ((bool)(this["ShowNotifications"]));
            }
            set {
                this["ShowNotifications"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool SupressNotificationsClient {
            get {
                return ((bool)(this["SupressNotificationsClient"]));
            }
            set {
                this["SupressNotificationsClient"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Seconds")]
        public global::Hyperwave.ViewModel.MailCheckIntervalUnit MailCheckUnit {
            get {
                return ((global::Hyperwave.ViewModel.MailCheckIntervalUnit)(this["MailCheckUnit"]));
            }
            set {
                this["MailCheckUnit"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Minutes")]
        public global::Hyperwave.ViewModel.MailCheckIntervalUnit BackgroundMailCheckUnit {
            get {
                return ((global::Hyperwave.ViewModel.MailCheckIntervalUnit)(this["BackgroundMailCheckUnit"]));
            }
            set {
                this["BackgroundMailCheckUnit"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Gallente")]
        public global::Hyperwave.Config.SkinStyle ColorScheme {
            get {
                return ((global::Hyperwave.Config.SkinStyle)(this["ColorScheme"]));
            }
            set {
                this["ColorScheme"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool SupressNotificationsFullscreen {
            get {
                return ((bool)(this["SupressNotificationsFullscreen"]));
            }
            set {
                this["SupressNotificationsFullscreen"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("90")]
        public decimal InitialBackgroundMailCheckInterval {
            get {
                return ((decimal)(this["InitialBackgroundMailCheckInterval"]));
            }
            set {
                this["InitialBackgroundMailCheckInterval"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Minutes")]
        public global::Hyperwave.ViewModel.MailCheckIntervalUnit InitialBackgroundMailCheckUnit {
            get {
                return ((global::Hyperwave.ViewModel.MailCheckIntervalUnit)(this["InitialBackgroundMailCheckUnit"]));
            }
            set {
                this["InitialBackgroundMailCheckUnit"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1")]
        public int UpdateBackgroundSettings {
            get {
                return ((int)(this["UpdateBackgroundSettings"]));
            }
            set {
                this["UpdateBackgroundSettings"] = value;
            }
        }
    }
}
