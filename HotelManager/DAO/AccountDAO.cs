﻿using HotelManager.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.DAO
{
    public class AccountDAO
    {
        private static AccountDAO instance;
        public string HashPass(string text)
        {
            MD5 md5 = MD5.Create();
            byte[] temp = Encoding.ASCII.GetBytes(text);
            byte[] hashData = md5.ComputeHash(temp);
            string hashPass = "";
            foreach (var item in hashData)
            {
                hashPass += item.ToString("x2");
            }
            return hashPass;
        }
        public bool Login(string userName, string passWord)
        {
            string hashPass = HashPass(passWord);
            string query = "exec USP_Login @userName , @passWord";
            DataTable data = DataProvider.Instance.ExecuteQuery(query, new object[] { userName, hashPass });
            return data.Rows.Count>0;
        }

        public static AccountDAO Instance
        {
            get { if (instance == null) instance = new AccountDAO();return instance; }
            private set => instance = value;
        }
        public bool InsertAccount(Account account)
        {
            string query = "EXEC USP_InsertStaff @user , @name , @pass , @idStaffType , @dateOfBirth , @sex , @address , @phoneNumber , @startDay";
            object[] parameter = new object[] {account.UserName, account.DisplayName, account.PassWord,
                                                account.IdStaffType, account.DateOfBirth, account.Sex,
                                                account.Address, account.PhoneNumber, account.StartDay};
            return DataProvider.Instance.ExecuteNoneQuery(query, parameter) > 0;
        }
        public bool UpdateAccount(Account account)
        {
            string query = "EXEC USP_UpdateStaff @user , @name , @idStaffType , @dateOfBirth , @sex , @address , @phoneNumber , @startDay";
            object[] parameter = new object[] {account.UserName, account.DisplayName,
                                                account.IdStaffType, account.DateOfBirth, account.Sex,
                                                account.Address, account.PhoneNumber, account.StartDay};
            return DataProvider.Instance.ExecuteNoneQuery(query, parameter) > 0;
        }
        public bool UpdatePassword(string user, string hashPass)
        {
            string query = "USP_UpdatePassword @user , @hashPass)";
            return DataProvider.Instance.ExecuteNoneQuery(query, new object[] { user, hashPass }) > 0;
        }
        private AccountDAO() { }

    }
}
