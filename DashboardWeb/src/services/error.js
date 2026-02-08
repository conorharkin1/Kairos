import { useToast } from 'vue-toastification'
const toast = useToast()

export default function DisplayErrorMessage(error) {
    if (error.response && error.response.data) {
        if (error.response.data?.detail) {
            toast.error(error.response.data.detail)
            return true;
        }
    }

    return false;
}
    