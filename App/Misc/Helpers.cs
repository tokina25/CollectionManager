﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using App.Interfaces;
using App.Models;
using App.Presenters.Forms;
using CollectionManagerExtensionsDll.Modules.DownloadManager.API;
using GuiComponents.Interfaces;

namespace App.Misc
{
    public static class Helpers
    {
        public static string StripInvalidCharacters(string name)
        {
            foreach (var invalidChar in Path.GetInvalidFileNameChars())
            {
                name = name.Replace(invalidChar.ToString(), string.Empty);
            }
            return name.Replace(".", string.Empty);
        }
        public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }

        public static LoginData GetLoginData(this ILoginFormView loginForm)
        {
            loginForm.ShowAndBlock();
            LoginData loginData = new LoginData();
            if (loginForm.ClickedLogin)
            {
                loginData.Username = loginForm.Login;
                loginData.Password = loginForm.Password;
            }

            return loginData.isValid() ? loginData : null;
        }

        public static string GetCollectionName(this ICollectionAddRenameForm form, Func<string, bool> isCollectionNameValid, string orginalName = "",
            bool isRenameForm = false)
        {
            ICollectionAddRenameModel model = new CollectionAddRenameModel(isCollectionNameValid,orginalName);
            new CollectionAddRenameFormPresenter(form, model);
            form.IsRenameForm = isRenameForm;
            form.CollectionRenameView.OrginalCollectionName = orginalName;
            form.ShowAndBlock();

            return model.NewCollectionNameIsValid ? model.NewCollectionName : "";
        }
    }
}