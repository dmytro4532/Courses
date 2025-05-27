import { Modal, message } from 'antd';
import api from '../../../api/axios';
import type { CourseResponse } from '../../../types';

interface DeleteCourseModalProps {
  course: CourseResponse | null;
  onClose: () => void;
  onSuccess: () => void;
}

export const DeleteCourseModal = ({ course, onClose, onSuccess }: DeleteCourseModalProps) => {
  const handleDelete = async () => {
    if (!course) return;

    try {
      await api.delete(`/api/courses/${course.id}`);
      message.success('Course deleted successfully');
      onSuccess();
    } catch (error) {
      message.error('Failed to delete course');
      console.error('Error deleting course:', error);
    }
  };

  return (
    <Modal
      title="Delete Course"
      open={!!course}
      onCancel={onClose}
      onOk={handleDelete}
      okText="Delete"
      okButtonProps={{
        danger: true,
      }}
      cancelText="Cancel"
    >
      <p>Are you sure you want to delete the course "{course?.title}"?</p>
      <p>This action cannot be undone.</p>
    </Modal>
  );
}; 