﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WCFService_2Way_20190140097
{
    [ServiceBehavior(ConcurrencyMode= ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.Single)]
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class ServiceCallBack : IServiceCallBack
    {
        //menyimpan data saat user online
        Dictionary<IClientCallBack, string> userList = new Dictionary<IClientCallBack, string>();
        public void gabung(string username)
        {
            //menampung user saat baru daftar
            IClientCallBack koneksiGabung = OperationContext.Current.GetCallbackChannel<IClientCallBack>();
            userList[koneksiGabung] = username;
        }

        public void kirimPesan(string pesan)
        {
            //mengirim data user dan pesan ke userlain
            IClientCallBack koneksiPesan = OperationContext.Current.GetCallbackChannel<IClientCallBack>();
            string user;
            if(!userList.TryGetValue(koneksiPesan, out user))
            {
                return;
            }
            foreach(IClientCallBack other in userList.Keys)
            {
                other.pesanKirim(user, pesan);
            }
        }
    }
}
