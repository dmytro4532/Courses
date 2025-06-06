import { Modal } from 'antd';
import { enqueueSnackbar } from 'notistack';
import api from '../../../api/axios';
import type { Test } from '../../../types';

interface DeleteTestModalProps {
  test: Test | null;
  onClose: () => void;
  onSuccess: () => void;
}

export const DeleteTestModal = ({ test, onClose, onSuccess }: DeleteTestModalProps) => {
  const handleDelete = async () => {
    if (!test) return;

    try {
      await api.delete(`/api/tests/${test.id}`);
      enqueueSnackbar('Test deleted successfully', { variant: 'success', autoHideDuration: 3000 });
      onSuccess();
    } catch (error: any) {
      enqueueSnackbar(error?.response?.data?.details || 'Failed to delete test', { variant: 'error', autoHideDuration: 3000 });
    }
  };

  return (
    <Modal
      title="Delete Test"
      open={!!test}
      onCancel={onClose}
      onOk={handleDelete}
      okText="Delete"
      okButtonProps={{
        danger: true,
      }}
      cancelText="Cancel"
    >
      <p>Are you sure you want to delete the test "{test?.title}"?</p>
      <p>This action cannot be undone.</p>
    </Modal>
  );
}; 