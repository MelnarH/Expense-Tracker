using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.DAL.Entities;

public class User : IdentityUser<int>
{
    public decimal Wallet { get; set; }
    public ICollection<Category> Categories { get; set; }
    public ICollection<Transaction> Transactions { get; set; }

    public User() { 
    
        Wallet = 0;
        Transactions = new List<Transaction>();
        Categories = new List<Category>();
    }   
}

public class Role : IdentityRole<int>
{

}
public class RoleClaim : IdentityRoleClaim<int>
{

}
public class UserRole : IdentityUserRole<int>
{

}
public class UserLogin : IdentityUserLogin<int>
{

}
public class UserClaim : IdentityUserClaim<int>
{

}
public class UserToken : IdentityUserToken<int>
{

}