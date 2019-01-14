using System;
using System.Collections.Generic;

namespace Jira
{
    public class JiraIssue
    {
        public string expand { get; set; }
        public Fields fields { get; set; }
        public string id { get; set; }
        public string key { get; set; }
        public string self { get; set; }

        public class Aggregateprogress
        {
            public Int64 percent { get; set; }
            public Int64 progress { get; set; }
            public Int64 total { get; set; }
        }

        public class AvatarUrls
        {
            public string __invalid_name__16x16 { get; set; }
            public string __invalid_name__24x24 { get; set; }
            public string __invalid_name__32x32 { get; set; }
            public string __invalid_name__48x48 { get; set; }
        }

        public class Assignee
        {
            public bool active { get; set; }
            public AvatarUrls avatarUrls { get; set; }
            public string displayName { get; set; }
            public string emailAddress { get; set; }
            public string key { get; set; }
            public string name { get; set; }
            public string self { get; set; }
            public string timeZone { get; set; }
        }

        public class Comment
        {
            public List<object> comments { get; set; }
            public Int64 maxResults { get; set; }
            public Int64 startAt { get; set; }
            public Int64 total { get; set; }
        }

        public class Component
        {
            public string id { get; set; }
            public string name { get; set; }
            public string self { get; set; }
        }

        public class AvatarUrls2
        {
            public string __invalid_name__16x16 { get; set; }
            public string __invalid_name__24x24 { get; set; }
            public string __invalid_name__32x32 { get; set; }
            public string __invalid_name__48x48 { get; set; }
        }

        public class Creator
        {
            public bool active { get; set; }
            public AvatarUrls2 avatarUrls { get; set; }
            public string displayName { get; set; }
            public string emailAddress { get; set; }
            public string key { get; set; }
            public string name { get; set; }
            public string self { get; set; }
            public string timeZone { get; set; }
        }

        public class Customfield10407
        {
            public string id { get; set; }
            public string self { get; set; }
            public string value { get; set; }
        }

        public class Customfield10500
        {
            public string id { get; set; }
            public string self { get; set; }
            public string value { get; set; }
        }

        public class Customfield10802
        {
            public string id { get; set; }
            public string self { get; set; }
            public string value { get; set; }
        }

        public class I18nErrorMessage
        {
            public string i18nKey { get; set; }
            public List<object> parameters { get; set; }
        }

        public class Customfield10900
        {
            public string errorMessage { get; set; }
            public I18nErrorMessage i18nErrorMessage { get; set; }
        }

        public class I18nErrorMessage2
        {
            public string i18nKey { get; set; }
            public List<object> parameters { get; set; }
        }

        public class Customfield10902
        {
            public string errorMessage { get; set; }
            public I18nErrorMessage2 i18nErrorMessage { get; set; }
        }

        public class I18nErrorMessage3
        {
            public string i18nKey { get; set; }
            public List<object> parameters { get; set; }
        }

        public class Customfield10903
        {
            public string errorMessage { get; set; }
            public I18nErrorMessage3 i18nErrorMessage { get; set; }
        }

        public class Customfield11100
        {
            public string id { get; set; }
            public string self { get; set; }
            public string value { get; set; }
        }

        public class Customfield11201
        {
            public string id { get; set; }
            public string self { get; set; }
            public string value { get; set; }
        }

        public class Customfield11401
        {
            public string id { get; set; }
            public string self { get; set; }
            public string value { get; set; }
        }

        public class Customfield11402
        {
            public string id { get; set; }
            public string self { get; set; }
            public string value { get; set; }
        }

        public class Customfield11501
        {
            public string id { get; set; }
            public string self { get; set; }
            public string value { get; set; }
        }

        public class Customfield11600
        {
            public string id { get; set; }
            public string self { get; set; }
            public string value { get; set; }
        }

        public class Customfield11700
        {
            public string id { get; set; }
            public string self { get; set; }
            public string value { get; set; }
        }

        public class Customfield11800
        {
            public string id { get; set; }
            public string self { get; set; }
            public string value { get; set; }
        }

        public class Customfield12201
        {
            public string id { get; set; }
            public string self { get; set; }
            public string value { get; set; }
        }

        public class Customfield12202
        {
            public string id { get; set; }
            public string self { get; set; }
            public string value { get; set; }
        }

        public class I18nErrorMessage4
        {
            public string i18nKey { get; set; }
            public List<object> parameters { get; set; }
        }

        public class Customfield12400
        {
            public string errorMessage { get; set; }
            public I18nErrorMessage4 i18nErrorMessage { get; set; }
        }

        public class I18nErrorMessage5
        {
            public string i18nKey { get; set; }
            public List<object> parameters { get; set; }
        }

        public class Customfield12401
        {
            public string errorMessage { get; set; }
            public I18nErrorMessage5 i18nErrorMessage { get; set; }
        }

        public class I18nErrorMessage6
        {
            public string i18nKey { get; set; }
            public List<object> parameters { get; set; }
        }

        public class Customfield12402
        {
            public string errorMessage { get; set; }
            public I18nErrorMessage6 i18nErrorMessage { get; set; }
        }

        public class I18nErrorMessage7
        {
            public string i18nKey { get; set; }
            public List<object> parameters { get; set; }
        }

        public class Customfield12408
        {
            public string errorMessage { get; set; }
            public I18nErrorMessage7 i18nErrorMessage { get; set; }
        }

        public class Customfield12503
        {
            public string id { get; set; }
            public string self { get; set; }
            public string value { get; set; }
        }

        public class Issuetype
        {
            public Int64 avatarId { get; set; }
            public string description { get; set; }
            public string iconUrl { get; set; }
            public string id { get; set; }
            public string name { get; set; }
            public string self { get; set; }
            public bool subtask { get; set; }
        }

        public class Priority
        {
            public string iconUrl { get; set; }
            public string id { get; set; }
            public string name { get; set; }
            public string self { get; set; }
        }

        public class StatusCategory
        {
            public string colorName { get; set; }
            public Int64 id { get; set; }
            public string key { get; set; }
            public string name { get; set; }
            public string self { get; set; }
        }

        public class Status
        {
            public string description { get; set; }
            public string iconUrl { get; set; }
            public string id { get; set; }
            public string name { get; set; }
            public string self { get; set; }
            public StatusCategory statusCategory { get; set; }
        }

        public class Fields2
        {
            public Issuetype issuetype { get; set; }
            public Priority priority { get; set; }
            public Status status { get; set; }
            public string summary { get; set; }
        }

        public class InwardIssue
        {
            public Fields2 fields { get; set; }
            public string id { get; set; }
            public string key { get; set; }
            public string self { get; set; }
        }

        public class Type
        {
            public string id { get; set; }
            public string inward { get; set; }
            public string name { get; set; }
            public string outward { get; set; }
            public string self { get; set; }
        }

        public class Issuelink
        {
            public string id { get; set; }
            public InwardIssue inwardIssue { get; set; }
            public string self { get; set; }
            public Type type { get; set; }
        }

        public class Issuetype2
        {
            public Int64 avatarId { get; set; }
            public string description { get; set; }
            public string iconUrl { get; set; }
            public string id { get; set; }
            public string name { get; set; }
            public string self { get; set; }
            public bool subtask { get; set; }
        }

        public class Priority2
        {
            public string iconUrl { get; set; }
            public string id { get; set; }
            public string name { get; set; }
            public string self { get; set; }
        }

        public class Progress
        {
            public Int64 percent { get; set; }
            public Int64 progress { get; set; }
            public Int64 total { get; set; }
        }

        public class AvatarUrls3
        {
            public string __invalid_name__16x16 { get; set; }
            public string __invalid_name__24x24 { get; set; }
            public string __invalid_name__32x32 { get; set; }
            public string __invalid_name__48x48 { get; set; }
        }

        public class ProjectCategory
        {
            public string description { get; set; }
            public string id { get; set; }
            public string name { get; set; }
            public string self { get; set; }
        }

        public class Project
        {
            public AvatarUrls3 avatarUrls { get; set; }
            public string id { get; set; }
            public string key { get; set; }
            public string name { get; set; }
            public ProjectCategory projectCategory { get; set; }
            public string self { get; set; }
        }

        public class AvatarUrls4
        {
            public string __invalid_name__16x16 { get; set; }
            public string __invalid_name__24x24 { get; set; }
            public string __invalid_name__32x32 { get; set; }
            public string __invalid_name__48x48 { get; set; }
        }

        public class Reporter
        {
            public bool active { get; set; }
            public AvatarUrls4 avatarUrls { get; set; }
            public string displayName { get; set; }
            public string emailAddress { get; set; }
            public string key { get; set; }
            public string name { get; set; }
            public string self { get; set; }
            public string timeZone { get; set; }
        }

        public class Resolution
        {
            public string description { get; set; }
            public string id { get; set; }
            public string name { get; set; }
            public string self { get; set; }
        }

        public class StatusCategory2
        {
            public string colorName { get; set; }
            public Int64 id { get; set; }
            public string key { get; set; }
            public string name { get; set; }
            public string self { get; set; }
        }

        public class Status2
        {
            public string description { get; set; }
            public string iconUrl { get; set; }
            public string id { get; set; }
            public string name { get; set; }
            public string self { get; set; }
            public StatusCategory2 statusCategory { get; set; }
        }

        public class Timetracking
        {
            public string remainingEstimate { get; set; }
            public Int64 remainingEstimateSeconds { get; set; }
            public string timeSpent { get; set; }
            public Int64 timeSpentSeconds { get; set; }
        }

        public class Votes
        {
            public bool hasVoted { get; set; }
            public string self { get; set; }
            public Int64 votes { get; set; }
        }

        public class Watches
        {
            public bool isWatching { get; set; }
            public string self { get; set; }
            public Int64 watchCount { get; set; }
        }

        public class Worklog
        {
            public Int64 maxResults { get; set; }
            public Int64 startAt { get; set; }
            public Int64 total { get; set; }
            public List<object> worklogs { get; set; }
        }

        public class Fields
        {
            public Aggregateprogress aggregateprogress { get; set; }
            public Int64 aggregatetimeestimate { get; set; }
            public object aggregatetimeoriginalestimate { get; set; }
            public Int64 aggregatetimespent { get; set; }
            public Assignee assignee { get; set; }
            public List<object> attachment { get; set; }
            public Comment comment { get; set; }
            public List<Component> components { get; set; }
            public string created { get; set; }
            public Creator creator { get; set; }
            public object customfield_10000 { get; set; }
            public object customfield_10001 { get; set; }
            public object customfield_10002 { get; set; }
            public string customfield_10004 { get; set; }
            public object customfield_10005 { get; set; }
            public object customfield_10006 { get; set; }
            public object customfield_10010 { get; set; }
            public string customfield_10200 { get; set; }
            public object customfield_10300 { get; set; }
            public object customfield_10301 { get; set; }
            public object customfield_10302 { get; set; }
            public double customfield_10404 { get; set; }
            public double customfield_10405 { get; set; }
            public double customfield_10406 { get; set; }
            public Customfield10407 customfield_10407 { get; set; }
            public object customfield_10408 { get; set; }
            public object customfield_10409 { get; set; }
            public object customfield_10410 { get; set; }
            public object customfield_10411 { get; set; }
            public Customfield10500 customfield_10500 { get; set; }
            public object customfield_10501 { get; set; }
            public object customfield_10600 { get; set; }
            public object customfield_10701 { get; set; }
            public object customfield_10800 { get; set; }
            public object customfield_10801 { get; set; }
            public List<Customfield10802> customfield_10802 { get; set; }
            public Customfield10900 customfield_10900 { get; set; }
            public List<object> customfield_10901 { get; set; }
            public Customfield10902 customfield_10902 { get; set; }
            public Customfield10903 customfield_10903 { get; set; }
            public object customfield_10904 { get; set; }
            public object customfield_10905 { get; set; }
            public object customfield_10906 { get; set; }
            public object customfield_10907 { get; set; }
            public double customfield_10909 { get; set; }
            public object customfield_10911 { get; set; }
            public object customfield_10912 { get; set; }
            public object customfield_10913 { get; set; }
            public object customfield_10915 { get; set; }
            public string customfield_11000 { get; set; }
            public Customfield11100 customfield_11100 { get; set; }
            public Customfield11201 customfield_11201 { get; set; }
            public object customfield_11206 { get; set; }
            public object customfield_11207 { get; set; }
            public object customfield_11208 { get; set; }
            public object customfield_11209 { get; set; }
            public object customfield_11210 { get; set; }
            public object customfield_11211 { get; set; }
            public object customfield_11212 { get; set; }
            public object customfield_11213 { get; set; }
            public object customfield_11214 { get; set; }
            public object customfield_11215 { get; set; }
            public object customfield_11300 { get; set; }
            public object customfield_11301 { get; set; }
            public object customfield_11302 { get; set; }
            public List<Customfield11401> customfield_11401 { get; set; }
            public Customfield11402 customfield_11402 { get; set; }
            public List<Customfield11501> customfield_11501 { get; set; }
            public List<Customfield11600> customfield_11600 { get; set; }
            public object customfield_11601 { get; set; }
            public object customfield_11602 { get; set; }
            public List<Customfield11700> customfield_11700 { get; set; }
            public object customfield_11701 { get; set; }
            public object customfield_11702 { get; set; }
            public object customfield_11703 { get; set; }
            public List<Customfield11800> customfield_11800 { get; set; }
            public string customfield_11901 { get; set; }
            public object customfield_11902 { get; set; }
            public object customfield_11905 { get; set; }
            public object customfield_12002 { get; set; }
            public object customfield_12003 { get; set; }
            public object customfield_12100 { get; set; }
            public object customfield_12101 { get; set; }
            public Customfield12201 customfield_12201 { get; set; }
            public Customfield12202 customfield_12202 { get; set; }
            public object customfield_12300 { get; set; }
            public object customfield_12301 { get; set; }
            public object customfield_12302 { get; set; }
            public object customfield_12303 { get; set; }
            public object customfield_12304 { get; set; }
            public Customfield12400 customfield_12400 { get; set; }
            public Customfield12401 customfield_12401 { get; set; }
            public Customfield12402 customfield_12402 { get; set; }
            public double customfield_12403 { get; set; }
            public object customfield_12404 { get; set; }
            public object customfield_12405 { get; set; }
            public object customfield_12406 { get; set; }
            public object customfield_12407 { get; set; }
            public Customfield12408 customfield_12408 { get; set; }
            public object customfield_12500 { get; set; }
            public object customfield_12501 { get; set; }
            public object customfield_12502 { get; set; }
            public List<Customfield12503> customfield_12503 { get; set; }
            public object customfield_12600 { get; set; }
            public object customfield_12601 { get; set; }
            public object customfield_12602 { get; set; }
            public string description { get; set; }
            public object duedate { get; set; }
            public object environment { get; set; }
            public List<object> fixVersions { get; set; }
            public List<Issuelink> issuelinks { get; set; }
            public Issuetype2 issuetype { get; set; }
            public List<object> labels { get; set; }
            public string lastViewed { get; set; }
            public Priority2 priority { get; set; }
            public Progress progress { get; set; }
            public Project project { get; set; }
            public Reporter reporter { get; set; }
            public Resolution resolution { get; set; }
            public string resolutiondate { get; set; }
            public Status2 status { get; set; }
            public List<object> subtasks { get; set; }
            public string summary { get; set; }
            public Int64 timeestimate { get; set; }
            public object timeoriginalestimate { get; set; }
            public Int64 timespent { get; set; }
            public Timetracking timetracking { get; set; }
            public string updated { get; set; }
            public List<object> versions { get; set; }
            public Votes votes { get; set; }
            public Watches watches { get; set; }
            public Worklog worklog { get; set; }
            public Int64 workratio { get; set; }
        }
    }
}
