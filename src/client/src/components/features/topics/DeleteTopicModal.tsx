import { Modal, message } from 'antd';
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
      message.success('Topic deleted successfully');
      onSuccess();
    } catch (error: any) {
      message.error(error?.response?.data?.details || 'Failed to delete topic');
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