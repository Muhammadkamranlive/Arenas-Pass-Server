﻿using Server.Models;
using Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace API.API.Transaction
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IAccount_Transaction_Service account_Transaction_Service;
        private readonly IGet_Tenant_Id_Service get_Tenant_Id_Service;
        private readonly IAccount_Balance_Service account_Balance_Service;
        private readonly IUser_Vault_Service userVault_Service;
        public TransactionController
        (
            IAccount_Transaction_Service _Transaction,
            IGet_Tenant_Id_Service       get_Tenant_Id,
            IAccount_Balance_Service     account_Balance,
            IUser_Vault_Service           userVault
        )
        {
            account_Transaction_Service = _Transaction;
            get_Tenant_Id_Service       = get_Tenant_Id;
            account_Balance_Service     = account_Balance;
            userVault_Service           = userVault;
        }

        //Api for Tansaction Records Credits and Debit and Closing Balance

        #region Transactions 
        /// <summary>
        /// TransactionType
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("TenantTransactions")]
        //[CustomAuthorize("Read")]
        public async Task<dynamic> TenantTransactions(int cardId)
        {
            try
            {
                return await account_Transaction_Service.Find(x => x.Card_Id == cardId);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// TransactionType
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("TransactionsByTenant")]
        [CustomAuthorize("Read")]
        public async Task<dynamic> TransactionsByTenant()
        {
            try
            {
                var TenantId = get_Tenant_Id_Service.GetTenantId(); 
                return await account_Transaction_Service.Find(x => x.Tenant_Id == TenantId.ToString());
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// TransactionType
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("TransactionsByCustomerEmail")]
        [CustomAuthorize("Read")]
        public async Task<dynamic> TransactionsByCustomerEmail(string email)
        {
            try
            {
                
                return await account_Transaction_Service.Find(x => x.Email==email);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        [Route("TransactionsByTenantId")]
        [CustomAuthorize("Read")]
        public async Task<dynamic> TransactionsByTenantId()
        {
            try
            {
                var tenantId = get_Tenant_Id_Service.GetTenantId();
                return await account_Transaction_Service.Find(x => x.Tenant_Id == tenantId.ToString());
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        
        /// <summary>
        /// TransactionType
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("OpeningBalanceByCustomerEmail")]
        [CustomAuthorize("Read")]
        public async Task<dynamic> OpeningBalanceByCustomerEmail(string Email)
        {
            try
            {
                
                return await account_Balance_Service.Find(x =>  x.Customer_Email == Email);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// TransactionType
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("VaultBalance")]
        //[CustomAuthorize("Read")]
        public async Task<dynamic> VaultBalance(string Email)
        {
            try
            {
                return await userVault_Service.Find(x => x.Email == Email);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// TransactionType
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("OpeningBalanceByTenant")]
        [CustomAuthorize("Read")]
        public async Task<dynamic> OpeningBalanceByTenant()
        {
            try
            {
                var TenantId = get_Tenant_Id_Service.GetTenantId();
                return await account_Balance_Service.Find(x => x.Tenant_Id == TenantId);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
