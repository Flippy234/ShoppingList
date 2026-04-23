using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Telephony.Euicc;
using Newtonsoft.Json;
using ShoppingList.Models;

namespace ShoppingList.Views;

public partial class NewAccountPage : ContentPage
{
    public NewAccountPage()
    {
        InitializeComponent();
        Title = "Create New Account";
    }

    async void CreateAccount_OnClicked(object sender, EventArgs e)
    {
        // // Do passwords match
        // if (txtPassword1.Text == txtPassword2.Text)
        // {
        //     
        // }
        // else if(txtPassword1.Text != txtPassword1.Text)
        // {
        //     
        // }
        //     
        
        // Is a valid email address = @ .
        // if (txtEmail.Text)
        
        // api stuff
        var data = JsonConvert.SerializeObject(new UserAccount(txtUser.Text, txtPassword1.Text, txtEmail.Text));

        var client = new HttpClient();
        var response = await client.PostAsync(new Uri("https://joewetzel.com/fvtc/account/createuser"),
            new StringContent(data, Encoding.UTF8, "application/json"));
        var AccountStatus = response.Content.ReadAsStringAsync().Result;

        AccountStatus = AccountStatus;
        
        // does the user exist
        if (AccountStatus == "user exists")
        {
            await DisplayAlert("Error", "Sorry this username has been taken!", "OK");
            return;
        }
        
        // is the email in use
        if (AccountStatus == "email exists")
        {
            await DisplayAlert("Error", "Sorry this email has already been used", "OK");
            return;
        }
        
        if (AccountStatus == "complete")
        {
            response = await client.PostAsync(new Uri("https://joewetzel.com/fvtc/account/login"),
                new StringContent(data, Encoding.UTF8, "application/json"));
            
            var SKey = response.Content.ReadAsStringAsync().Result;

            if (!string.IsNullOrEmpty(SKey) || Skey.Length > 50)
            {
                App.SessionKey = Skey;
                Navigation.PopModalAsync();
            }
            else
            {
                await DisplayAlert("Error", "Sorry there was an error creating your account!", "OK");
                return;
            }
        }
    }
}