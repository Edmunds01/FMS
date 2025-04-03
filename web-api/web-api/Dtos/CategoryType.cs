using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace web_api.Dtos;

public enum CategoryType
{
    Income = 1,
    Expense = 2,
}
