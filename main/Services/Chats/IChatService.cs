using ValuedInBE.DataControls.Paging;
using ValuedInBE.Models.DTOs.Requests.Chats;
using ValuedInBE.Models.DTOs.Responses.Chats;

namespace ValuedInBE.Services.Chats
{
    public interface IChatService
    {
        bool AnyUnreadChats();

        Page<ChatLatestInfo> GetLatestChats(ChatPageRequest chatPage);

        ChatLatestInfo GetLatestInfoOnChat(long id);

        Page<MessageDTO> GetMessages(MessagePageRequest messagePage);

        ChatLatestInfo FetchOrCreateAChat(NewChatRequest newChatRequest);

        MessageDTO CreateNewMessage(NewMessage newMessage);

        
    }
}
