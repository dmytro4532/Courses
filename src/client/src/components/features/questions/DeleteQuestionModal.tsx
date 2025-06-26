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
      enqueueSnackbar('Питання успішно видалено', { variant: 'success', autoHideDuration: 3000 });
      onSuccess();
    } catch (error: any) {
      enqueueSnackbar(error?.response?.data?.details || 'Не вдалося видалити питання', { variant: 'error', autoHideDuration: 3000 });
    }
  };

  return (
    <Modal
      title="Видалити питання"
      open={!!question}
      onCancel={onClose}
      onOk={handleDelete}
      okText="Видалити"
      okButtonProps={{
        danger: true,
      }}
      cancelText="Скасувати"
    >
      <p>Ви впевнені, що хочете видалити це питання?</p>
      <p>Цю дію неможливо скасувати.</p>
    </Modal>
  );
}; 