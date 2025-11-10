export function getTimeStampLongFormat() {
    const options = {
        dateStyle: 'medium',
        timeStyle: 'short',
    }
    return new Date().toLocaleString('en-IE', options)
}
