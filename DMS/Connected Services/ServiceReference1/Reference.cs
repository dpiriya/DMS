﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DMS.ServiceReference1 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference1.WSDatatableSoap")]
    public interface WSDatatableSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetData_Office", ReplyAction="*")]
        void GetData_Office();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetData_Office", ReplyAction="*")]
        System.Threading.Tasks.Task GetData_OfficeAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetData_Recruit", ReplyAction="*")]
        void GetData_Recruit();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetData_Recruit", ReplyAction="*")]
        System.Threading.Tasks.Task GetData_RecruitAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetData_Accounts", ReplyAction="*")]
        void GetData_Accounts();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetData_Accounts", ReplyAction="*")]
        System.Threading.Tasks.Task GetData_AccountsAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetData_Purchase", ReplyAction="*")]
        void GetData_Purchase();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetData_Purchase", ReplyAction="*")]
        System.Threading.Tasks.Task GetData_PurchaseAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface WSDatatableSoapChannel : DMS.ServiceReference1.WSDatatableSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class WSDatatableSoapClient : System.ServiceModel.ClientBase<DMS.ServiceReference1.WSDatatableSoap>, DMS.ServiceReference1.WSDatatableSoap {
        
        public WSDatatableSoapClient() {
        }
        
        public WSDatatableSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public WSDatatableSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WSDatatableSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WSDatatableSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void GetData_Office() {
            base.Channel.GetData_Office();
        }
        
        public System.Threading.Tasks.Task GetData_OfficeAsync() {
            return base.Channel.GetData_OfficeAsync();
        }
        
        public void GetData_Recruit() {
            base.Channel.GetData_Recruit();
        }
        
        public System.Threading.Tasks.Task GetData_RecruitAsync() {
            return base.Channel.GetData_RecruitAsync();
        }
        
        public void GetData_Accounts() {
            base.Channel.GetData_Accounts();
        }
        
        public System.Threading.Tasks.Task GetData_AccountsAsync() {
            return base.Channel.GetData_AccountsAsync();
        }
        
        public void GetData_Purchase() {
            base.Channel.GetData_Purchase();
        }
        
        public System.Threading.Tasks.Task GetData_PurchaseAsync() {
            return base.Channel.GetData_PurchaseAsync();
        }
    }
}
