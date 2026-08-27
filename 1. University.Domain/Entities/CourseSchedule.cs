using System;

namespace University.Domain.Entities;

public class CourseSchedule
{
    public int Id { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }

    public int CourseOfferingId { get; set; }
    public CourseOffering CourseOffering { get; set; }
}
