using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.BLL.DTO;

public class DTOResponse<T>
{
    public T Data { get; set; }
    public bool Success => Errors.Count == 0;
    public List<string> Errors { get; set; } = new List<string>();

    public DTOResponse(string error)
    {
        Errors = new List<string> { error };
    }
    public DTOResponse(T data)
    {
        Data = data;
        Errors = new List<string>();
    }
}