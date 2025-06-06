import { Modal } from 'antd';
import { enqueueSnackbar } from 'notistack';
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
      enqueueSnackbar('Question deleted successfully', { variant: 'success', autoHideDuration: 3000 });
      onSuccess();
    } catch (error: any) {
      enqueueSnackbar(error?.response?.data?.details || 'Failed to delete question', { variant: 'error', autoHideDuration: 3000 });
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