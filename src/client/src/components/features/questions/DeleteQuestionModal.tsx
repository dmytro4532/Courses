import { Modal, message } from 'antd';
import api from '../../../api/axios';
import type { Question } from '../../../types';

interface DeleteQuestionModalProps {
  question: Question | null;
  onClose: () => void;
  onSuccess: () => void;
}

export const DeleteQuestionModal = ({ question, onClose, onSuccess }: DeleteQuestionModalProps) => {
  const handleDelete = async () => {
    if (!question) return;

    try {
      await api.delete(`/api/questions/${question.id}`);
      message.success('Question deleted successfully');
      onSuccess();
    } catch (error: any) {
      message.error(error?.response?.data?.details || 'Failed to delete question');
    }
  };

  return (
    <Modal
      title="Delete Question"
      open={!!question}
      onCancel={onClose}
      onOk={handleDelete}
      okText="Delete"
      okButtonProps={{
        danger: true,
      }}
      cancelText="Cancel"
    >
      <p>Are you sure you want to delete this question?</p>
      <p>This action cannot be undone.</p>
    </Modal>
  );
}; 