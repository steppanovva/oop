using System;
using Isu.Services;
using Isu.Tools;
using NUnit.Framework;

namespace Isu.Tests
{
    [TestFixture]
    public class Tests
    {
        private IIsuService _isuService;

        public Services.Isu TestIsu { get; set; }

        [Test]
        public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
        {
            var group = TestIsu.AddGroup(new GroupName() {NumberOfCourse = 2, NumberOfGroup = 5});
            var student = TestIsu.AddStudent(group, "Van");
            Assert.AreEqual(student.Group.GroupName, group.GroupName);
            Assert.IsTrue(group.Students.Exists(x => x.Id == student.Id));
        }

        [Test]
        public void ReachMaxStudentPerGroup_ThrowException()
        {
            Assert.Catch<IsuException>(() =>
            {
                var group = TestIsu.AddGroup(new GroupName() {NumberOfCourse = 1, NumberOfGroup = 5});
                for (var i = 0; i < 5; i++)
                    TestIsu.AddStudent(group, i.ToString());
            });
        }

        [Test]
        public void CreateGroupWithInvalidName_ThrowException()
        {
            Assert.Catch<IsuException>(() =>
            {
                var group = TestIsu.AddGroup(new GroupName() {NumberOfCourse = 0, NumberOfGroup = 5});
            });
        }

        [Test]
        public void TransferStudentToAnotherGroup_GroupChanged()
        {
            var group1 = TestIsu.AddGroup(new GroupName() {NumberOfCourse = 2, NumberOfGroup = 6});
            var group2 = TestIsu.AddGroup(new GroupName() {NumberOfCourse = 2, NumberOfGroup = 7});
            var student = TestIsu.AddStudent(group1, "Billy");
            TestIsu.ChangeStudentGroup(student, group2);
            Assert.AreEqual(student.Group.GroupName, group2.GroupName);
            Assert.IsTrue(group2.Students.Exists(x => x.Id == student.Id));
        }
    }

    public class TestAttribute : Attribute
    {
    }
}