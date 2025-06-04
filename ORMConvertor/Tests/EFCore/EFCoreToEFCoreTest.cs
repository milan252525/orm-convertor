﻿using AbstractWrappers;
using EFCoreWrappers;
using Model;
using SampleData;

namespace Tests.EFCore;

public class EFCoreToEFCoreTest
{
    [Fact]
    public void EFCoreToEFCoreOverall()
    {
        AbstractEntityBuilder builder = new EFCoreEntityBuilder();
        var entityParser = new EFCoreEntityParser(builder);
        entityParser.Parse(CustomerSampleEFCore.Entity);

        var results = builder.Build();
        var entityOutput = results.Single(x => x.ContentType == ContentType.CSharpEntity);

        Assert.Multiple(() =>
        {
            Assert.Equal(ContentType.CSharpEntity, entityOutput.ContentType);
            Assert.Equal(CustomerSampleEFCore.Entity, entityOutput.Content, ignoreLineEndingDifferences: true);
        });
    }
}
