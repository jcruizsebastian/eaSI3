import { WeekJiraIssues } from '../WeekJiraIssues';
import { Calendar } from '../Calendar';

export interface UserCredentialsState {
    Weekissues: WeekJiraIssues[];
    loadedJira: boolean;
    loadingJira: boolean;
    calendar: Calendar;
    calendarLoaded: boolean;
    selectedWeek?: string;
    todoOk: boolean;
    loading: boolean;
    availableHours: Number;
    popup: boolean;
    popup_error: boolean;
    popup_data: String[];
}