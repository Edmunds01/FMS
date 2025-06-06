﻿namespace web_api.Services.Interfaces;

public interface IAccountService
{
    IEnumerable<Dtos.Account> GetUserAccounts();

    Task CreateNewAccountAsync(Dtos.NewAccount accountRaw);

    Task SaveAccountIconAsync(long accountId, string icon);

    Task SaveAccountNameAsync(long accountId, string name);

    Task DeleteAccountAsync(long accountId);

    Task ValidateIsUserAccountAsync(long accountId);
}
