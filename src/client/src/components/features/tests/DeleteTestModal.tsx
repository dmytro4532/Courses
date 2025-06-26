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
      enqueueSnackbar('Тест успішно видалено', { variant: 'success', autoHideDuration: 3000 });
      onSuccess();
    } catch (error: any) {
      enqueueSnackbar(error?.response?.data?.details || 'Не вдалося видалити тест', { variant: 'error', autoHideDuration: 3000 });
    }
  };

  return (
    <Modal
      title="Видалити тест"
      open={!!test}
      onCancel={onClose}
      onOk={handleDelete}
      okText="Видалити"
      okButtonProps={{
        danger: true,
      }}
      cancelText="Скасувати"
    >
      <p>Ви впевнені, що хочете видалити тест "{test?.title}"?</p>
      <p>Цю дію неможливо скасувати.</p>
    </Modal>
  );
}; 