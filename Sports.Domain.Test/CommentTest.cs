using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Sports.Domain.Test
{
    public class CommentTest
    {
        Comment comment;

        [TestInitialize]
        public void SetUp()
        {
            comment = new Comment();
        }
    }
}
