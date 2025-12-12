import dayjs from "dayjs";
import utc from "dayjs/plugin/utc";
import timezone from "dayjs/plugin/timezone";

dayjs.extend(utc);
dayjs.extend(timezone);

const APP_TIMEZONE = "Europe/Dublin";

export const convertToAppTimezone = (date) => {
    if (!date) return dayjs();
    return dayjs.utc(date).tz(APP_TIMEZONE);
};

export const convertToUTC = (dateStr, format = "YYYY-MM-DD HH:mm") => {
    if (!dateStr) return null;
    return dayjs.tz(dateStr, format, APP_TIMEZONE).utc().toISOString();
};