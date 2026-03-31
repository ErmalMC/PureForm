import toast from 'react-hot-toast';

export const notificationService = {
    success: (message, options = {}) => {
        toast.success(message, {
            duration: 4000,
            position: 'top-right',
            ...options,
        });
    },

    error: (message, options = {}) => {
        toast.error(message, {
            duration: 4000,
            position: 'top-right',
            ...options,
        });
    },

    loading: (message, options = {}) => {
        return toast.loading(message, {
            position: 'top-right',
            ...options,
        });
    },

    promise: (promise, messages, options = {}) => {
        return toast.promise(
            promise,
            {
                loading: messages.loading || 'Loading...',
                success: messages.success || 'Success!',
                error: messages.error || 'Error!',
            },
            {
                position: 'top-right',
                ...options,
            }
        );
    },

    dismiss: (toastId) => {
        if (toastId) {
            toast.dismiss(toastId);
        } else {
            toast.dismiss();
        }
    },
};

export default notificationService;

