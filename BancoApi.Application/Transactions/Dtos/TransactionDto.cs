using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoApi.Application.Transactions.Dtos;
public record TransactionDto
{
    public Guid? Id { get; set; }
    public Guid? OriginWalletId { get; set; }
    public Guid DestinationWalletId { get; set; }
    public decimal Value { get; set; }
    public DateTime? TransactionDate { get; set; }

}
