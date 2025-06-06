import { Modal } from 'antd';
import { enqueueSnackbar } from 'notistack';
import api from '../../../api/axios';
import type { Topic } from '../../../types';

interface DeleteTopicModalProps {
  topic: Topic | null;
  onClose: () => void;
  onSuccess: () => void;
}

export const DeleteTopicModal = ({ topic, onClose, onSuccess }: DeleteTopicModalProps) => {
  const handleDelete = async () => {
    if (!topic) return;

    try {
      await api.delete(`/api/topics/${topic.id}`);
      enqueueSnackbar('Topic deleted successfully', { variant: 'success', autoHideDuration: 3000 });
      onSuccess();
    } catch (error: any) {
      enqueueSnackbar(error?.response?.data?.details || 'Failed to delete topic', { variant: 'error', autoHideDuration: 3000 });
    }
  };

  return (
    <Modal
      title="Delete Topic"
      open={!!topic}
      onCancel={onClose}
      onOk={handleDelete}
      okText="Delete"
      okButtonProps={{
        danger: true,
      }}
      cancelText="Cancel"
    >
      <p>Are you sure you want to delete the topic "{topic?.title}"?</p>
      <p>This action cannot be undone.</p>
    </Modal>
  );
}; 