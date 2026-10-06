using System;
using System.Collections.Generic;
using System.Text;

namespace PurityUIDashborad.Infrastructure;

public interface IInvoiceService
{
    public Task<IEnumerable<InvoiceDto>> GetInvoicesAsync();
}

internal sealed class MockInvoiceService : IInvoiceService
{
    public async Task<IEnumerable<InvoiceDto>> GetInvoicesAsync()
    {
        await Task.Delay(2456);

        return [
            new("MS-43135", new(2020, 3, 1, 0, 0, 0, TimeSpan.Zero), 180),
            new("RV-12347", new(2021, 2, 10, 0, 0, 0, TimeSpan.Zero), 180),
            new("FB-43135", new(2020, 4, 5, 0, 0, 0, TimeSpan.Zero), 180),
            new("QW-43135", new(2019, 6, 26, 0, 0, 0, TimeSpan.Zero), 180),
            new("AR-43135", new(2019, 3, 1, 0, 0, 0, TimeSpan.Zero), 180),
        ];
    }
}

public record InvoiceDto(string Id, DateTimeOffset DateTime, double Invoice);