import { Fragment } from 'react';
import { Dialog, Transition } from '@headlessui/react';
import { XMarkIcon } from '@heroicons/react/24/outline';
import { motion } from 'framer-motion';

const AnimatedModal = ({
    isOpen,
    onClose,
    title,
    children,
    footer,
    size = 'md',
    closeButton = true,
}) => {
    const sizeClasses = {
        sm: 'max-w-sm',
        md: 'max-w-md',
        lg: 'max-w-lg',
        xl: 'max-w-xl',
        '2xl': 'max-w-2xl',
    };

    return (
        <Transition show={isOpen} as={Fragment}>
            <Dialog as="div" className="relative z-50" onClose={onClose}>
                <Transition.Child
                    as={Fragment}
                    enter="ease-out duration-300"
                    enterFrom="opacity-0"
                    enterTo="opacity-100"
                    leave="ease-in duration-200"
                    leaveFrom="opacity-100"
                    leaveTo="opacity-0"
                >
                    <div className="fixed inset-0 bg-black/50 backdrop-blur-sm" />
                </Transition.Child>

                <div className="fixed inset-0 overflow-y-auto">
                    <div className="flex min-h-full items-center justify-center p-4">
                        <Transition.Child
                            as={Fragment}
                            enter="ease-out duration-300"
                            enterFrom="opacity-0 scale-95"
                            enterTo="opacity-100 scale-100"
                            leave="ease-in duration-200"
                            leaveFrom="opacity-100 scale-100"
                            leaveTo="opacity-0 scale-95"
                        >
                            <motion.div
                                initial={{ opacity: 0, scale: 0.95 }}
                                animate={{ opacity: 1, scale: 1 }}
                                exit={{ opacity: 0, scale: 0.95 }}
                                transition={{ duration: 0.3 }}
                            >
                                <Dialog.Panel
                                    className={`w-full ${sizeClasses[size]} transform overflow-hidden rounded-2xl bg-white p-6 shadow-2xl transition-all`}
                                >
                                    <div className="flex items-start justify-between mb-4">
                                        {title && (
                                            <Dialog.Title className="text-2xl font-bold text-gray-900">
                                                {title}
                                            </Dialog.Title>
                                        )}
                                        {closeButton && (
                                            <button
                                                onClick={onClose}
                                                className="text-gray-400 hover:text-gray-600 transition-colors"
                                            >
                                                <XMarkIcon className="h-6 w-6" />
                                            </button>
                                        )}
                                    </div>

                                    <div className="py-4">
                                        {children}
                                    </div>

                                    {footer && (
                                        <div className="mt-6 flex gap-3 justify-end">
                                            {footer}
                                        </div>
                                    )}
                                </Dialog.Panel>
                            </motion.div>
                        </Transition.Child>
                    </div>
                </div>
            </Dialog>
        </Transition>
    );
};

export default AnimatedModal;

