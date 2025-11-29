export function getTimeStampLongFormat(date) {
    const options = {
        dateStyle: 'medium',
        timeStyle: 'short',
    }
    return new Date(date).toLocaleString('en-IE', options)
}
