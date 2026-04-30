using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
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
        var username = txtEmail.Text?.Trim();
        var password = txtPassword1.Text;
        var email = txtEmail.Text?.Trim();
        
        // if password invalid 
        if(txtPassword1.Text != txtPassword2.Text)
        {
            await DisplayAlert("Error", "Sorry, passwords does not match", "OK");
            return;
        }
        
        //Is a valid email address = @ .
        if (!(txtEmail.Text.Contains("@") && txtEmail.Text.Contains(".") 
                                          && txtEmail.Text.IndexOf('@') < txtEmail.Text.LastIndexOf('.')))
        {
            await DisplayAlert("Error", "Invalid email address", "OK");
            return;
        }

        
        // api stuff
        var data = JsonConvert.SerializeObject(new UserAccount(username, password, email));

        var client = new HttpClient();
        var response = await client.PostAsync(new Uri("https://joewetzel.com/fvtc/account/createuser"),
            new StringContent(data, Encoding.UTF8, "application/json"));
        
        // var AccountStatus = (await response.Content.ReadAsStringAsync().Result;
        var AccountStatus = (await response.Content.ReadAsStringAsync());
        // await DisplayAlert("DEBUG", $"'{AccountStatus}'", "OK");
        
        // does the user exist
        if (AccountStatus=="user exists")
        {
            await DisplayAlert("Error", "Sorry this username has been taken!", "OK");
            return;
        }
        
        // is the email in use
        if (AccountStatus =="email exists")
        {
            await DisplayAlert("Error", "Sorry this email has already been used", "OK");
            return;
        }
        
        if (AccountStatus == "complete")
        {
            response = await client.PostAsync(new Uri("https://joewetzel.com/fvtc/account/login"),
                    new StringContent(data, Encoding.UTF8, "application/json"));
                     
                var SKey = response.Content.ReadAsStringAsync().Result;
         
                if (!string.IsNullOrEmpty(SKey) && SKey.Length < 50)
                {
                    App.SessionKey = SKey;
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