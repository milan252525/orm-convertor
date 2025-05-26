﻿using AbstractWrappers;
using EFCoreWrappers;
using Model;
using Tests.SampleData;

namespace Tests.EFCore;

public class EFCoreToEFCoreTest
{
    [Fact]
    public void EFCoreToEFCoreOverall()
    {
        AbstractEntityBuilder builder = new EFCoreEntityBuilder();
        var entityParser = new EFCoreEntityParser(builder);
        entityParser.Parse(CustomerMapEFCore.Source);

        var results = builder.Build();
        var entityOutput = results.Single(x => x.ContentType == ContentType.CSharp);

        Assert.Multiple(() =>
        {
            Assert.Equal(ContentType.CSharp, entityOutput.ContentType);
            Assert.Equal(CustomerMapEFCore.Source, entityOutput.Content, ignoreLineEndingDifferences: true);
        });
    }
}
