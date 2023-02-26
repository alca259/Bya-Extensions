namespace LibraryTests;

[TestClass]
public class DateRangeTest
{
    [TestMethod]
    public void TestIterationDays()
    {
        var range1 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 23), new DateTime(2020, 4, 23, 16, 43, 56));
        var elements = range1.Step(f => f.AddDays(1)).ToArray();
        Assert.IsTrue(elements.Length == 4);
    }

    [TestMethod]
    public void TestIterationWeeks()
    {
        var range1 = new DateRange(new DateTime(2020, 4, 1, 23, 54, 13), new DateTime(2020, 4, 28, 9, 53, 12));
        var elements = range1.Step(f => f.AddDays(7)).ToArray();
        Assert.IsTrue(elements.Length == 4);
    }

    [TestMethod]
    public void TestIterationDaysWithHours()
    {
        var range1 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 23), new DateTime(2020, 4, 23, 16, 43, 56))
        {
            UseHours = true
        };

        var elements1 = range1.Step(f => f.AddDays(1)).ToArray();
        Assert.IsTrue(elements1.Length == 4);

        range1.End = new DateTime(2020, 4, 23, 11, 43, 56);
        var elements2 = range1.Step(f => f.AddDays(1)).ToArray();
        Assert.IsTrue(elements2.Length == 3);
    }

    [TestMethod]
    public void TestDateInRange()
    {
        var range1 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 23), new DateTime(2020, 4, 23, 16, 43, 56))
        {
            UseHours = false
        };

        var testDate1 = new DateTime(2020, 4, 20, 15, 23, 1);
        var testDate2 = new DateTime(2020, 4, 20, 11, 46, 32);
        var testDate3 = new DateTime(2020, 4, 21);
        var testDate4 = new DateTime(2020, 4, 23);
        var testDate5 = new DateTime(2020, 4, 23, 17, 34, 23);
        var testDate6 = new DateTime(2020, 4, 24);
        var testDate7 = new DateTime(2020, 4, 25);

        Assert.IsTrue(range1.Inside(testDate1));
        Assert.IsTrue(range1.Inside(testDate2));
        Assert.IsTrue(range1.Inside(testDate3));
        Assert.IsTrue(range1.Inside(testDate4));
        Assert.IsTrue(range1.Inside(testDate5));
        Assert.IsFalse(range1.Inside(testDate6));
        Assert.IsFalse(range1.Inside(testDate7));
    }

    [TestMethod]
    public void TestDateInRangeWithHours()
    {
        var range1 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 23), new DateTime(2020, 4, 23, 16, 43, 56))
        {
            UseHours = true
        };

        var testDate1 = new DateTime(2020, 4, 20, 15, 23, 1);
        var testDate2 = new DateTime(2020, 4, 20, 11, 46, 32);
        var testDate3 = new DateTime(2020, 4, 21);
        var testDate4 = new DateTime(2020, 4, 23);
        var testDate5 = new DateTime(2020, 4, 23, 17, 34, 23);
        var testDate6 = new DateTime(2020, 4, 24);
        var testDate7 = new DateTime(2020, 4, 25);

        Assert.IsTrue(range1.Inside(testDate1));
        Assert.IsFalse(range1.Inside(testDate2));
        Assert.IsTrue(range1.Inside(testDate3));
        Assert.IsTrue(range1.Inside(testDate4));
        Assert.IsFalse(range1.Inside(testDate5));
        Assert.IsFalse(range1.Inside(testDate6));
        Assert.IsFalse(range1.Inside(testDate7));
    }

    [TestMethod]
    public void TestRangeInsideRange()
    {
        var range1 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 23), new DateTime(2020, 4, 23, 16, 43, 56))
        {
            UseHours = false
        };

        var testRange1 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 23), new DateTime(2020, 4, 23, 16, 43, 56));
        var testRange2 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 22), new DateTime(2020, 4, 23, 16, 43, 56));
        var testRange3 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 24), new DateTime(2020, 4, 23, 16, 43, 55));
        var testRange4 = new DateRange(new DateTime(2020, 4, 21, 12, 53, 23), new DateTime(2020, 4, 22, 16, 43, 56));
        var testRange5 = new DateRange(new DateTime(2020, 4, 19, 12, 53, 23), new DateTime(2020, 4, 22, 16, 43, 56));
        var testRange6 = new DateRange(new DateTime(2020, 4, 21, 12, 53, 23), new DateTime(2020, 4, 24, 16, 43, 56));

        Assert.IsTrue(range1.Inside(testRange1));
        Assert.IsTrue(range1.Inside(testRange2));
        Assert.IsTrue(range1.Inside(testRange3));
        Assert.IsTrue(range1.Inside(testRange4));
        Assert.IsFalse(range1.Inside(testRange5));
        Assert.IsFalse(range1.Inside(testRange6));
    }

    [TestMethod]
    public void TestRangeInsideRangeWithHours()
    {
        var range1 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 23), new DateTime(2020, 4, 23, 16, 43, 56))
        {
            UseHours = true
        };

        var testRange1 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 23), new DateTime(2020, 4, 23, 16, 43, 56));
        var testRange2 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 22), new DateTime(2020, 4, 23, 16, 43, 56));
        var testRange3 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 24), new DateTime(2020, 4, 23, 16, 43, 55));
        var testRange4 = new DateRange(new DateTime(2020, 4, 21, 12, 53, 23), new DateTime(2020, 4, 22, 16, 43, 56));
        var testRange5 = new DateRange(new DateTime(2020, 4, 19, 12, 53, 23), new DateTime(2020, 4, 22, 16, 43, 56));
        var testRange6 = new DateRange(new DateTime(2020, 4, 21, 12, 53, 23), new DateTime(2020, 4, 24, 16, 43, 56));

        Assert.IsTrue(range1.Inside(testRange1));
        Assert.IsFalse(range1.Inside(testRange2));
        Assert.IsTrue(range1.Inside(testRange3));
        Assert.IsTrue(range1.Inside(testRange4));
        Assert.IsFalse(range1.Inside(testRange5));
        Assert.IsFalse(range1.Inside(testRange6));
    }

    [TestMethod]
    public void TestRangeStartInside()
    {
        var range1 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 23), new DateTime(2020, 4, 23, 16, 43, 56))
        {
            UseHours = false,
            InclusiveStart = true
        };

        var testRange1 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 23), new DateTime(2020, 4, 23, 16, 43, 56));
        var testRange2 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 22), new DateTime(2020, 4, 23, 16, 43, 56));
        var testRange3 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 24), new DateTime(2020, 4, 23, 16, 43, 55));
        var testRange4 = new DateRange(new DateTime(2020, 4, 21, 12, 53, 23), new DateTime(2020, 4, 22, 16, 43, 56));
        var testRange5 = new DateRange(new DateTime(2020, 4, 19, 12, 53, 23), new DateTime(2020, 4, 22, 16, 43, 56));
        var testRange6 = new DateRange(new DateTime(2020, 4, 21, 12, 53, 23), new DateTime(2020, 4, 24, 16, 43, 56));

        Assert.IsTrue(range1.StartInside(testRange1));
        Assert.IsTrue(range1.StartInside(testRange2));
        Assert.IsTrue(range1.StartInside(testRange3));
        Assert.IsTrue(range1.StartInside(testRange4));
        Assert.IsFalse(range1.StartInside(testRange5));
        Assert.IsTrue(range1.StartInside(testRange6));
    }

    [TestMethod]
    public void TestRangeStartInsideWithHours()
    {
        var range1 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 23), new DateTime(2020, 4, 23, 16, 43, 56))
        {
            UseHours = true,
            InclusiveStart = true
        };

        var testRange1 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 23), new DateTime(2020, 4, 23, 16, 43, 56));
        var testRange2 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 22), new DateTime(2020, 4, 23, 16, 43, 56));
        var testRange3 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 24), new DateTime(2020, 4, 23, 16, 43, 55));
        var testRange4 = new DateRange(new DateTime(2020, 4, 21, 12, 53, 23), new DateTime(2020, 4, 22, 16, 43, 56));
        var testRange5 = new DateRange(new DateTime(2020, 4, 19, 12, 53, 23), new DateTime(2020, 4, 22, 16, 43, 56));
        var testRange6 = new DateRange(new DateTime(2020, 4, 21, 12, 53, 23), new DateTime(2020, 4, 24, 16, 43, 56));
        var testRange7 = new DateRange(new DateTime(2020, 4, 24, 12, 53, 23), new DateTime(2020, 4, 25, 16, 43, 56));

        Assert.IsTrue(range1.StartInside(testRange1));
        Assert.IsFalse(range1.StartInside(testRange2));
        Assert.IsTrue(range1.StartInside(testRange3));
        Assert.IsTrue(range1.StartInside(testRange4));
        Assert.IsFalse(range1.StartInside(testRange5));
        Assert.IsTrue(range1.StartInside(testRange6));
        Assert.IsFalse(range1.StartInside(testRange7));
    }

    [TestMethod]
    public void TestRangeStartInsideEndOutside()
    {
        var range1 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 23), new DateTime(2020, 4, 23, 16, 43, 56))
        {
            UseHours = false,
            InclusiveStart = true
        };

        var testRange1 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 23), new DateTime(2020, 4, 23, 16, 43, 56));
        var testRange2 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 22), new DateTime(2020, 4, 23, 16, 43, 56));
        var testRange3 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 24), new DateTime(2020, 4, 23, 16, 43, 55));
        var testRange4 = new DateRange(new DateTime(2020, 4, 21, 12, 53, 23), new DateTime(2020, 4, 22, 16, 43, 56));
        var testRange5 = new DateRange(new DateTime(2020, 4, 19, 12, 53, 23), new DateTime(2020, 4, 22, 16, 43, 56));
        var testRange6 = new DateRange(new DateTime(2020, 4, 21, 12, 53, 23), new DateTime(2020, 4, 24, 16, 43, 56));

        Assert.IsFalse(range1.StartInsideEndsOutside(testRange1));
        Assert.IsFalse(range1.StartInsideEndsOutside(testRange2));
        Assert.IsFalse(range1.StartInsideEndsOutside(testRange3));
        Assert.IsFalse(range1.StartInsideEndsOutside(testRange4));
        Assert.IsFalse(range1.StartInsideEndsOutside(testRange5));
        Assert.IsTrue(range1.StartInsideEndsOutside(testRange6));
    }

    [TestMethod]
    public void TestRangeStartInsideEndOutsideWithHours()
    {
        var range1 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 23), new DateTime(2020, 4, 23, 16, 43, 56))
        {
            UseHours = true,
            InclusiveStart = true
        };

        var testRange1 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 23), new DateTime(2020, 4, 23, 16, 43, 56));
        var testRange2 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 22), new DateTime(2020, 4, 23, 16, 43, 56));
        var testRange3 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 24), new DateTime(2020, 4, 23, 16, 43, 55));
        var testRange4 = new DateRange(new DateTime(2020, 4, 21, 12, 53, 23), new DateTime(2020, 4, 22, 16, 43, 56));
        var testRange5 = new DateRange(new DateTime(2020, 4, 19, 12, 53, 23), new DateTime(2020, 4, 22, 16, 43, 56));
        var testRange6 = new DateRange(new DateTime(2020, 4, 21, 12, 53, 23), new DateTime(2020, 4, 23, 16, 43, 57));
        var testRange7 = new DateRange(new DateTime(2020, 4, 23, 12, 53, 23), new DateTime(2020, 4, 25, 16, 43, 56));
        var testRange8 = new DateRange(new DateTime(2020, 4, 21, 12, 53, 23), new DateTime(2020, 4, 23, 16, 43, 56));

        Assert.IsFalse(range1.StartInsideEndsOutside(testRange1));
        Assert.IsFalse(range1.StartInsideEndsOutside(testRange2));
        Assert.IsFalse(range1.StartInsideEndsOutside(testRange3));
        Assert.IsFalse(range1.StartInsideEndsOutside(testRange4));
        Assert.IsFalse(range1.StartInsideEndsOutside(testRange5));
        Assert.IsTrue(range1.StartInsideEndsOutside(testRange6));
        Assert.IsTrue(range1.StartInsideEndsOutside(testRange7));
        Assert.IsFalse(range1.StartInsideEndsOutside(testRange8));
    }

    [TestMethod]
    public void TestRangeEndInside()
    {
        var range1 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 23), new DateTime(2020, 4, 23, 16, 43, 56))
        {
            UseHours = false,
            InclusiveEnd = true
        };

        var testRange1 = new DateRange(new DateTime(2020, 4, 19, 12, 53, 23), new DateTime(2020, 4, 20, 11, 43, 56));
        var testRange2 = new DateRange(new DateTime(2020, 4, 18, 12, 53, 22), new DateTime(2020, 4, 19, 16, 43, 56));
        var testRange3 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 24), new DateTime(2020, 4, 24, 16, 43, 55));
        var testRange4 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 24), new DateTime(2020, 4, 22, 16, 43, 55));

        Assert.IsTrue(range1.EndsInside(testRange1));
        Assert.IsFalse(range1.EndsInside(testRange2));
        Assert.IsFalse(range1.EndsInside(testRange3));
        Assert.IsTrue(range1.EndsInside(testRange4));
    }

    [TestMethod]
    public void TestRangeEndInsideWithHours()
    {
        var range1 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 23), new DateTime(2020, 4, 23, 16, 43, 56))
        {
            UseHours = true,
            InclusiveEnd = true
        };

        var testRange1 = new DateRange(new DateTime(2020, 4, 19, 12, 53, 23), new DateTime(2020, 4, 20, 11, 43, 56));
        var testRange2 = new DateRange(new DateTime(2020, 4, 18, 12, 53, 22), new DateTime(2020, 4, 19, 16, 43, 56));
        var testRange3 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 24), new DateTime(2020, 4, 24, 16, 43, 55));
        var testRange4 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 24), new DateTime(2020, 4, 22, 16, 43, 55));

        Assert.IsFalse(range1.EndsInside(testRange1));
        Assert.IsFalse(range1.EndsInside(testRange2));
        Assert.IsFalse(range1.EndsInside(testRange3));
        Assert.IsTrue(range1.EndsInside(testRange4));
    }

    [TestMethod]
    public void TestRangeStartOutsideEndInside()
    {
        var range1 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 23), new DateTime(2020, 4, 23, 16, 43, 56))
        {
            UseHours = false,
            InclusiveEnd = true
        };

        var testRange1 = new DateRange(new DateTime(2020, 4, 19, 12, 53, 23), new DateTime(2020, 4, 20, 11, 43, 56));
        var testRange2 = new DateRange(new DateTime(2020, 4, 18, 12, 53, 22), new DateTime(2020, 4, 19, 16, 43, 56));
        var testRange3 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 24), new DateTime(2020, 4, 24, 16, 43, 55));
        var testRange4 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 24), new DateTime(2020, 4, 22, 16, 43, 55));

        Assert.IsTrue(range1.StartOutsideEndsInside(testRange1));
        Assert.IsFalse(range1.StartOutsideEndsInside(testRange2));
        Assert.IsFalse(range1.StartOutsideEndsInside(testRange3));
        Assert.IsFalse(range1.StartOutsideEndsInside(testRange4));
    }

    [TestMethod]
    public void TestRangeStartOutsideEndInsideWithHours()
    {
        var range1 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 23), new DateTime(2020, 4, 23, 16, 43, 56))
        {
            UseHours = true,
            InclusiveEnd = true
        };

        var testRange1 = new DateRange(new DateTime(2020, 4, 19, 12, 53, 23), new DateTime(2020, 4, 20, 11, 43, 56));
        var testRange2 = new DateRange(new DateTime(2020, 4, 18, 12, 53, 22), new DateTime(2020, 4, 19, 16, 43, 56));
        var testRange3 = new DateRange(new DateTime(2020, 4, 19, 12, 53, 24), new DateTime(2020, 4, 24, 16, 43, 55));
        var testRange4 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 24), new DateTime(2020, 4, 22, 16, 43, 55));
        var testRange5 = new DateRange(new DateTime(2020, 4, 19, 12, 53, 23), new DateTime(2020, 4, 20, 13, 43, 56));

        Assert.IsFalse(range1.StartOutsideEndsInside(testRange1));
        Assert.IsFalse(range1.StartOutsideEndsInside(testRange2));
        Assert.IsFalse(range1.StartOutsideEndsInside(testRange3));
        Assert.IsFalse(range1.StartOutsideEndsInside(testRange4));
        Assert.IsTrue(range1.StartOutsideEndsInside(testRange5));
    }

    [TestMethod]
    public void TestRangeOverlap()
    {
        var range1 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 23), new DateTime(2020, 4, 23, 16, 43, 56))
        {
            UseHours = false,
            InclusiveStart = true,
            InclusiveEnd = true
        };

        var testRange1 = new DateRange(new DateTime(2020, 4, 19, 12, 53, 23), new DateTime(2020, 4, 20, 11, 43, 56));
        var testRange2 = new DateRange(new DateTime(2020, 4, 18, 12, 53, 22), new DateTime(2020, 4, 19, 16, 43, 56));
        var testRange3 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 24), new DateTime(2020, 4, 24, 16, 43, 55));
        var testRange4 = new DateRange(new DateTime(2020, 4, 19, 12, 53, 24), new DateTime(2020, 4, 24, 16, 43, 55));

        Assert.IsFalse(range1.Overlap(testRange1));
        Assert.IsFalse(range1.Overlap(testRange2));
        Assert.IsFalse(range1.Overlap(testRange3));
        Assert.IsTrue(range1.Overlap(testRange4));
    }

    [TestMethod]
    public void TestRangeOverlapWithHours()
    {
        var range1 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 23), new DateTime(2020, 4, 23, 16, 43, 56))
        {
            UseHours = true,
            InclusiveStart = true,
            InclusiveEnd = true
        };

        var testRange1 = new DateRange(new DateTime(2020, 4, 19, 12, 53, 23), new DateTime(2020, 4, 23, 16, 43, 57));
        var testRange2 = new DateRange(new DateTime(2020, 4, 18, 12, 53, 22), new DateTime(2020, 4, 19, 16, 43, 56));
        var testRange3 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 24), new DateTime(2020, 4, 24, 16, 43, 55));
        var testRange4 = new DateRange(new DateTime(2020, 4, 19, 12, 53, 24), new DateTime(2020, 4, 24, 16, 43, 55));
        var testRange5 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 23), new DateTime(2020, 4, 23, 16, 43, 56));

        Assert.IsTrue(range1.Overlap(testRange1));
        Assert.IsFalse(range1.Overlap(testRange2));
        Assert.IsFalse(range1.Overlap(testRange3));
        Assert.IsTrue(range1.Overlap(testRange4));
        Assert.IsFalse(range1.Overlap(testRange5));
    }

    [TestMethod]
    public void TestRangeCollision()
    {
        var range1 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 23), new DateTime(2020, 4, 23, 16, 43, 56))
        {
            UseHours = false,
            InclusiveStart = true,
            InclusiveEnd = true
        };

        var testRange1 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 23), new DateTime(2020, 4, 23, 16, 43, 56));
        var testRange2 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 22), new DateTime(2020, 4, 23, 16, 43, 56));
        var testRange3 = new DateRange(new DateTime(2020, 4, 19, 12, 53, 23), new DateTime(2020, 4, 23, 16, 43, 57));
        var testRange4 = new DateRange(new DateTime(2020, 4, 18, 12, 53, 22), new DateTime(2020, 4, 19, 16, 43, 56));
        var testRange5 = new DateRange(new DateTime(2020, 4, 24, 12, 53, 22), new DateTime(2020, 4, 25, 16, 43, 56));
        var testRange6 = new DateRange(new DateTime(2020, 4, 22, 12, 53, 22), new DateTime(2020, 4, 25, 16, 43, 56));
        var testRange7 = new DateRange(new DateTime(2020, 4, 18, 12, 53, 22), new DateTime(2020, 4, 22, 16, 43, 56));

        Assert.IsTrue(range1.Collision(testRange1));
        Assert.IsTrue(range1.Collision(testRange2));
        Assert.IsTrue(range1.Collision(testRange3));
        Assert.IsFalse(range1.Collision(testRange4));
        Assert.IsFalse(range1.Collision(testRange5));
        Assert.IsTrue(range1.Collision(testRange6));
        Assert.IsTrue(range1.Collision(testRange7));
    }

    [TestMethod]
    public void TestRangeCollisionWithHours()
    {
        var range1 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 23), new DateTime(2020, 4, 23, 16, 43, 56))
        {
            UseHours = true,
            InclusiveStart = true,
            InclusiveEnd = true
        };

        var testRange1 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 23), new DateTime(2020, 4, 23, 16, 43, 56));
        var testRange2 = new DateRange(new DateTime(2020, 4, 20, 12, 53, 22), new DateTime(2020, 4, 23, 16, 43, 56));
        var testRange3 = new DateRange(new DateTime(2020, 4, 19, 12, 53, 23), new DateTime(2020, 4, 23, 16, 43, 57));
        var testRange4 = new DateRange(new DateTime(2020, 4, 18, 12, 53, 22), new DateTime(2020, 4, 19, 16, 43, 56));
        var testRange5 = new DateRange(new DateTime(2020, 4, 24, 12, 53, 22), new DateTime(2020, 4, 25, 16, 43, 56));
        var testRange6 = new DateRange(new DateTime(2020, 4, 22, 12, 53, 22), new DateTime(2020, 4, 25, 16, 43, 56));
        var testRange7 = new DateRange(new DateTime(2020, 4, 18, 12, 53, 22), new DateTime(2020, 4, 22, 16, 43, 56));
        var testRange8 = new DateRange(new DateTime(2020, 4, 23, 16, 43, 57), new DateTime(2020, 4, 23, 16, 43, 58));

        Assert.IsTrue(range1.Collision(testRange1));
        Assert.IsTrue(range1.Collision(testRange2));
        Assert.IsTrue(range1.Collision(testRange3));
        Assert.IsFalse(range1.Collision(testRange4));
        Assert.IsFalse(range1.Collision(testRange5));
        Assert.IsTrue(range1.Collision(testRange6));
        Assert.IsTrue(range1.Collision(testRange7));
        Assert.IsFalse(range1.Collision(testRange8));
    }

    [TestMethod]
    public void TestRangeCollisionWithHoursNoInclusive()
    {
        var range1 = new DateRange(new DateTime(1900, 1, 1, 12, 0, 0), new DateTime(1900, 1, 1, 16, 0, 0))
        {
            UseHours = true,
            InclusiveStart = false,
            InclusiveEnd = false
        };

        var testRange1 = new DateRange(new DateTime(1900, 1, 1, 11, 0, 0), new DateTime(1900, 1, 1, 17, 0, 0)); // Empieza fuera, termina fuera
        var testRange3 = new DateRange(new DateTime(1900, 1, 1, 12, 30, 0), new DateTime(1900, 1, 1, 15, 30, 0)); // Dentro
        var testRange6 = new DateRange(new DateTime(1900, 1, 1, 12, 30, 0), new DateTime(1900, 1, 1, 16, 30, 0)); // Empieza dentro, termina fuera
        var testRange7 = new DateRange(new DateTime(1900, 1, 1, 11, 30, 0), new DateTime(1900, 1, 1, 15, 30, 0)); // Empieza fuera, termina dentro
        var testRange8 = new DateRange(new DateTime(1900, 1, 1, 16, 30, 0), new DateTime(1900, 1, 1, 17, 30, 0)); // Totalmente fuera, sin colision

        Assert.IsTrue(range1.Collision(testRange1));
        Assert.IsTrue(range1.Collision(testRange3));
        Assert.IsTrue(range1.Collision(testRange6));
        Assert.IsTrue(range1.Collision(testRange7));
        Assert.IsFalse(range1.Collision(testRange8));

        var testRange4 = new DateRange(new DateTime(1900, 1, 1, 11, 0, 0), new DateTime(1900, 1, 1, 12, 0, 0)); // Empieza fuera, extremo inicio
        var testRange5 = new DateRange(new DateTime(1900, 1, 1, 16, 0, 0), new DateTime(1900, 1, 1, 16, 30, 0)); // Extremo final, termina fuera

        Assert.IsFalse(range1.Collision(testRange4));
        Assert.IsFalse(range1.Collision(testRange5));

        var testRange2 = new DateRange(new DateTime(1900, 1, 1, 12, 0, 0), new DateTime(1900, 1, 1, 16, 0, 0)); // Ambos Extremos
        Assert.IsFalse(range1.Inside(testRange2));
        Assert.IsFalse(range1.StartInside(testRange2));
        Assert.IsFalse(range1.EndsInside(testRange2));
        Assert.IsTrue(range1.Overlap(testRange2));
        Assert.IsTrue(range1.Collision(testRange2));

        var testRange9 = new DateRange(new DateTime(1900, 1, 1, 12, 0, 0), new DateTime(1900, 1, 1, 15, 30, 0)); // Extremo inicio, termina dentro
        var testRange10 = new DateRange(new DateTime(1900, 1, 1, 12, 0, 0), new DateTime(1900, 1, 1, 16, 30, 0)); // Extremo inicio, termina fuera

        Assert.IsTrue(range1.Collision(testRange9));
        Assert.IsTrue(range1.Collision(testRange10));

        var testRange11 = new DateRange(new DateTime(1900, 1, 1, 8, 0, 0), new DateTime(1900, 1, 1, 16, 0, 0)); // Empieza fuera, termina en extremo final

        Assert.IsTrue(range1.Collision(testRange11));
    }
}
