﻿using System.Collections.Generic;
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

namespace irods_Csharp
{
    internal static class MessageType
    {
        public const string CONNECT = "RODS_CONNECT";
        public const string DISCONNECT = "RODS_DISCONNECT";
        public const string API_REQ = "RODS_API_REQ";
        public const string API_REPLY = "RODS_API_REPLY";
        public const string REAUTH = "RODS_REAUTH";
        public const string VERSION = "RODS_VERSION";
    }

    //https://github.com/irods/irods/blob/master/lib/api/include/apiNumberData.h
    internal static class ApiNumberData
    {
        //500 - 599 - Internal File I/O API calls
        public const int FILE_CREATE_AN = 500;
        public const int FILE_OPEN_AN = 501;
        public const int FILE_WRITE_AN = 502;
        public const int FILE_CLOSE_AN = 503;
        public const int FILE_LSEEK_AN = 504;
        public const int FILE_READ_AN = 505;
        public const int FILE_UNLINK_AN = 506;
        public const int FILE_MKDIR_AN = 507;
        public const int FILE_CHMOD_AN = 508;
        public const int FILE_RMDIR_AN = 509;
        public const int FILE_STAT_AN = 510;
        public const int FILE_FSTAT_AN = 511;
        public const int FILE_FSYNC_AN = 512;

        public const int FILE_STAGE_AN = 513;
        public const int FILE_GET_FS_FREE_SPACE_AN = 514;
        public const int FILE_OPENDIR_AN = 515;
        public const int FILE_CLOSEDIR_AN = 516;
        public const int FILE_READDIR_AN = 517;
        public const int FILE_PUT_AN = 518;
        public const int FILE_GET_AN = 519;
        public const int FILE_CHKSUM_AN = 520;
        public const int CHK_N_V_PATH_PERM_AN = 521;
        public const int FILE_RENAME_AN = 522;
        public const int FILE_TRUNCATE_AN = 523;
        public const int FILE_STAGE_TO_CACHE_AN = 524;
        public const int FILE_SYNC_TO_ARCH_AN = 525;

        // 600 - 699 - Object File I/O API calls
        public const int DATA_OBJ_CREATE_AN = 601;
        public const int DATA_OBJ_OPEN_AN = 602;
        public const int DATA_OBJ_PUT_AN = 606;
        public const int DATA_PUT_AN = 607;
        public const int DATA_OBJ_GET_AN = 608;
        public const int DATA_GET_AN = 609;
        public const int DATA_COPY_AN = 611;
        public const int SIMPLE_QUERY_AN = 614;
        public const int DATA_OBJ_UNLINK_AN = 615;
        public const int REG_DATA_OBJ_AN = 619;
        public const int UNREG_DATA_OBJ_AN = 620;
        public const int REG_REPLICA_AN = 621;
        public const int MOD_DATA_OBJ_META_AN = 622;
        public const int RULE_EXEC_SUBMIT_AN = 623;
        public const int RULE_EXEC_DEL_AN = 624;
        public const int EXEC_MY_RULE_AN = 625;
        public const int OPR_COMPLETE_AN = 626;
        public const int DATA_OBJ_RENAME_AN = 627;
        public const int DATA_OBJ_RSYNC_AN = 628;
        public const int DATA_OBJ_CHKSUM_AN = 629;
        public const int PHY_PATH_REG_AN = 630;
        public const int DATA_OBJ_TRIM_AN = 632;
        public const int OBJ_STAT_AN = 633;
        public const int SUB_STRUCT_FILE_CREATE_AN = 635;
        public const int SUB_STRUCT_FILE_OPEN_AN = 636;
        public const int SUB_STRUCT_FILE_READ_AN = 637;
        public const int SUB_STRUCT_FILE_WRITE_AN = 638;
        public const int SUB_STRUCT_FILE_CLOSE_AN = 639;
        public const int SUB_STRUCT_FILE_UNLINK_AN = 640;
        public const int SUB_STRUCT_FILE_STAT_AN = 641;
        public const int SUB_STRUCT_FILE_FSTAT_AN = 642;
        public const int SUB_STRUCT_FILE_LSEEK_AN = 643;
        public const int SUB_STRUCT_FILE_RENAME_AN = 644;
        public const int QUERY_SPEC_COLL_AN = 645;
        public const int SUB_STRUCT_FILE_MKDIR_AN = 647;
        public const int SUB_STRUCT_FILE_RMDIR_AN = 648;
        public const int SUB_STRUCT_FILE_OPENDIR_AN = 649;
        public const int SUB_STRUCT_FILE_READDIR_AN = 650;
        public const int SUB_STRUCT_FILE_CLOSEDIR_AN = 651;
        public const int DATA_OBJ_TRUNCATE_AN = 652;
        public const int SUB_STRUCT_FILE_TRUNCATE_AN = 653;
        public const int GET_XMSG_TICKET_AN = 654;
        public const int SEND_XMSG_AN = 655;
        public const int RCV_XMSG_AN = 656;
        public const int SUB_STRUCT_FILE_GET_AN = 657;
        public const int SUB_STRUCT_FILE_PUT_AN = 658;
        public const int SYNC_MOUNTED_COLL_AN = 659;
        public const int STRUCT_FILE_SYNC_AN = 660;
        public const int CLOSE_COLLECTION_AN = 661;
        public const int STRUCT_FILE_EXTRACT_AN = 664;
        public const int STRUCT_FILE_EXT_AND_REG_AN = 665;
        public const int STRUCT_FILE_BUNDLE_AN = 666;
        public const int CHK_OBJ_PERM_AND_STAT_AN = 667;
        public const int GET_REMOTE_ZONE_RESC_AN = 668;
        public const int DATA_OBJ_OPEN_AND_STAT_AN = 669;
        public const int L3_FILE_GET_SINGLE_BUF_AN = 670;
        public const int L3_FILE_PUT_SINGLE_BUF_AN = 671;
        public const int DATA_OBJ_CREATE_AND_STAT_AN = 672;
        public const int DATA_OBJ_CLOSE_AN = 673;
        public const int DATA_OBJ_LSEEK_AN = 674;
        public const int DATA_OBJ_READ_AN = 675;
        public const int DATA_OBJ_WRITE_AN = 676;
        public const int COLL_REPL_AN = 677;
        public const int OPEN_COLLECTION_AN = 678;
        public const int RM_COLL_AN = 679;
        public const int MOD_COLL_AN = 680;
        public const int COLL_CREATE_AN = 681;
        public const int DATA_OBJ_UNLOCK_AN = 682;
        public const int REG_COLL_AN = 683;
        public const int PHY_BUNDLE_COLL_AN = 684;
        public const int UNBUN_AND_REG_PHY_BUNFILE_AN = 685;
        public const int GET_HOST_FOR_PUT_AN = 686;
        public const int GET_RESC_QUOTA_AN = 687;
        public const int BULK_DATA_OBJ_REG_AN = 688;
        public const int BULK_DATA_OBJ_PUT_AN = 689;
        public const int PROC_STAT_AN = 690;
        public const int STREAM_READ_AN = 691;
        public const int EXEC_CMD_AN = 692;
        public const int STREAM_CLOSE_AN = 693;
        public const int GET_HOST_FOR_GET_AN = 694;
        public const int DATA_OBJ_REPL_AN = 695;
        public const int DATA_OBJ_COPY_AN = 696;
        public const int DATA_OBJ_PHYMV_AN = 697;
        public const int DATA_OBJ_FSYNC_AN = 698;
        public const int DATA_OBJ_LOCK_AN = 699; // JMC - backport 4599

        // 700 - 799 - Metadata API calls
        public const int GET_MISC_SVR_INFO_AN = 700;
        public const int GENERAL_ADMIN_AN = 701;
        public const int GEN_QUERY_AN = 702;
        public const int AUTH_REQUEST_AN = 703;
        public const int AUTH_RESPONSE_AN = 704;
        public const int AUTH_CHECK_AN = 705;
        public const int MOD_AVU_METADATA_AN = 706;
        public const int MOD_ACCESS_CONTROL_AN = 707;
        public const int RULE_EXEC_MOD_AN = 708;
        public const int GET_TEMP_PASSWORD_AN = 709;
        public const int GENERAL_UPDATE_AN = 710;
        public const int READ_COLLECTION_AN = 713;
        public const int USER_ADMIN_AN = 714;
        public const int GENERAL_ROW_INSERT_AN = 715;
        public const int GENERAL_ROW_PURGE_AN = 716;
        public const int END_TRANSACTION_AN = 718;
        public const int DATABASE_RESC_OPEN_AN = 719;
        public const int DATABASE_OBJ_CONTROL_AN = 720;
        public const int DATABASE_RESC_CLOSE_AN = 721;
        public const int SPECIFIC_QUERY_AN = 722;
        public const int TICKET_ADMIN_AN = 723;
        public const int GET_TEMP_PASSWORD_FOR_OTHER_AN = 724;
        public const int PAM_AUTH_REQUEST_AN = 725;
        public const int GET_LIMITED_PASSWORD_AN = 726;

        // 1100 - 1200 - SSL API calls
        public const int SSL_START_AN = 1100;
        public const int SSL_END_AN = 1101;

        public const int AUTH_PLUG_REQ_AN = 1201;
        public const int AUTH_PLUG_RESP_AN = 1202;
        public const int GET_HIER_FOR_RESC_AN = 1203;
        public const int GET_HIER_FROM_LEAF_ID_AN = 1204;
        public const int SET_RR_CTX_AN = 1205;
        public const int EXEC_RULE_EXPRESSION_AN = 1206;

        public const int SERVER_REPORT_AN = 10204;
        public const int ZONE_REPORT_AN = 10205;
        public const int CLIENT_HINTS_AN = 10215;

        // clang-format on
    };
    internal static class Table
    {
        //https://github.com/irods/irods/blob/master/lib/core/include/rodsErrorTable.h
        public static Dictionary<int, string> ApiErrorData = new Dictionary<int, string>()
        {
            {-1000,"SYS_SOCK_OPEN_ERR"},
            {-1100,"SYS_SOCK_LISTEN_ERR"},
            {-2000,"SYS_SOCK_BIND_ERR"},
            {-3000,"SYS_SOCK_ACCEPT_ERR"},
            {-4000,"SYS_HEADER_READ_LEN_ERR"},
            {-5000,"SYS_HEADER_WRITE_LEN_ERR"},
            {-6000,"SYS_HEADER_TYPE_LEN_ERR"},
            {-7000,"SYS_CAUGHT_SIGNAL"},
            {-8000,"SYS_GETSTARTUP_PACK_ERR"},
            {-9000,"SYS_EXCEED_CONNECT_CNT"},
            {-10000,"SYS_USER_NOT_ALLOWED_TO_CONN"},
            {-11000,"SYS_READ_MSG_BODY_INPUT_ERR"},
            {-12000,"SYS_UNMATCHED_API_NUM"},
            {-13000,"SYS_NO_API_PRIV"},
            {-14000,"SYS_API_INPUT_ERR"},
            {-15000,"SYS_PACK_INSTRUCT_FORMAT_ERR"},
            {-16000,"SYS_MALLOC_ERR"},
            {-17000,"SYS_GET_HOSTNAME_ERR"},
            {-18000,"SYS_OUT_OF_FILE_DESC"},
            {-19000,"SYS_FILE_DESC_OUT_OF_RANGE"},
            {-20000,"SYS_UNRECOGNIZED_REMOTE_FLAG"},
            {-21000,"SYS_INVALID_SERVER_HOST"},
            {-22000,"SYS_SVR_TO_SVR_CONNECT_FAILED"},
            {-23000,"SYS_BAD_FILE_DESCRIPTOR"},
            {-24000,"SYS_INTERNAL_NULL_INPUT_ERR"},
            {-25000,"SYS_CONFIG_FILE_ERR"},
            {-26000,"SYS_INVALID_ZONE_NAME"},
            {-27000,"SYS_COPY_LEN_ERR"},
            {-28000,"SYS_PORT_COOKIE_ERR"},
            {-29000,"SYS_KEY_VAL_TABLE_ERR"},
            {-30000,"SYS_INVALID_RESC_TYPE"},
            {-31000,"SYS_INVALID_FILE_PATH"},
            {-32000,"SYS_INVALID_RESC_INPUT"},
            {-33000,"SYS_INVALID_PORTAL_OPR"},
            {-35000,"SYS_INVALID_OPR_TYPE"},
            {-36000,"SYS_NO_PATH_PERMISSION"},
            {-37000,"SYS_NO_ICAT_SERVER_ERR"},
            {-38000,"SYS_AGENT_INIT_ERR"},
            {-39000,"SYS_PROXYUSER_NO_PRIV"},
            {-40000,"SYS_NO_DATA_OBJ_PERMISSION"},
            {-41000,"SYS_DELETE_DISALLOWED"},
            {-42000,"SYS_OPEN_REI_FILE_ERR"},
            {-43000,"SYS_NO_RCAT_SERVER_ERR"},
            {-44000,"SYS_UNMATCH_PACK_INSTRUCTI_NAME"},
            {-45000,"SYS_SVR_TO_CLI_MSI_NO_EXIST"},
            {-46000,"SYS_COPY_ALREADY_IN_RESC"},
            {-47000,"SYS_RECONN_OPR_MISMATCH"},
            {-48000,"SYS_INPUT_PERM_OUT_OF_RANGE"},
            {-49000,"SYS_FORK_ERROR"},
            {-50000,"SYS_PIPE_ERROR"},
            {-51000,"SYS_EXEC_CMD_STATUS_SZ_ERROR"},
            {-52000,"SYS_PATH_IS_NOT_A_FILE"},
            {-53000,"SYS_UNMATCHED_SPEC_COLL_TYPE"},
            {-54000,"SYS_TOO_MANY_QUERY_RESULT"},
            {-55000,"SYS_SPEC_COLL_NOT_IN_CACHE"},
            {-56000,"SYS_SPEC_COLL_OBJ_NOT_EXIST"},
            {-57000,"SYS_REG_OBJ_IN_SPEC_COLL"},
            {-58000,"SYS_DEST_SPEC_COLL_SUB_EXIST"},
            {-59000,"SYS_SRC_DEST_SPEC_COLL_CONFLICT"},
            {-60000,"SYS_UNKNOWN_SPEC_COLL_CLASS"},
            {-61000,"END_OF_LIFE_SYS_DUPLICATE_XMSG_TICKET"},
            {-62000,"END_OF_LIFE_SYS_UNMATCHED_XMSG_TICKET"},
            {-63000,"END_OF_LIFE_SYS_NO_XMSG_FOR_MSG_NUMBER"},
            {-64000,"SYS_COLLINFO_2_FORMAT_ERR"},
            {-65000,"SYS_CACHE_STRUCT_FILE_RESC_ERR"},
            {-66000,"SYS_NOT_SUPPORTED"},
            {-67000,"SYS_TAR_STRUCT_FILE_EXTRACT_ERR"},
            {-68000,"SYS_STRUCT_FILE_DESC_ERR"},
            {-69000,"SYS_TAR_OPEN_ERR"},
            {-70000,"SYS_TAR_EXTRACT_ALL_ERR"},
            {-71000,"SYS_TAR_CLOSE_ERR"},
            {-72000,"SYS_STRUCT_FILE_PATH_ERR"},
            {-73000,"SYS_MOUNT_MOUNTED_COLL_ERR"},
            {-74000,"SYS_COLL_NOT_MOUNTED_ERR"},
            {-75000,"SYS_STRUCT_FILE_BUSY_ERR"},
            {-76000,"SYS_STRUCT_FILE_INMOUNTED_COLL"},
            {-77000,"SYS_COPY_NOT_EXIST_IN_RESC"},
            {-78000,"SYS_RESC_DOES_NOT_EXIST"},
            {-79000,"SYS_COLLECTION_NOT_EMPTY"},
            {-80000,"SYS_OBJ_TYPE_NOT_STRUCT_FILE"},
            {-81000,"SYS_WRONG_RESC_POLICY_FOR_BUN_OPR"},
            {-82000,"SYS_DIR_IN_VAULT_NOT_EMPTY"},
            {-83000,"SYS_OPR_FLAG_NOT_SUPPORT"},
            {-84000,"SYS_TAR_APPEND_ERR"},
            {-85000,"SYS_INVALID_PROTOCOL_TYPE"},
            {-86000,"SYS_UDP_CONNECT_ERR"},
            {-89000,"SYS_UDP_TRANSFER_ERR"},
            {-90000,"SYS_UDP_NO_SUPPORT_ERR"},
            {-91000,"SYS_READ_MSG_BODY_LEN_ERR"},
            {-92000,"CROSS_ZONE_SOCK_CONNECT_ERR"},
            {-93000,"SYS_NO_FREE_RE_THREAD"},
            {-94000,"SYS_BAD_RE_THREAD_INX"},
            {-95000,"SYS_CANT_DIRECTLY_ACC_COMPOUND_RESC"},
            {-96000,"SYS_SRC_DEST_RESC_COMPOUND_TYPE"},
            {-97000,"SYS_CACHE_RESC_NOT_ON_SAME_HOST"},
            {-98000,"SYS_NO_CACHE_RESC_IN_GRP"},
            {-99000,"SYS_UNMATCHED_RESC_IN_RESC_GRP"},
            {-100000,"SYS_CANT_MV_BUNDLE_DATA_TO_TRASH"},
            {-101000,"SYS_CANT_MV_BUNDLE_DATA_BY_COPY"},
            {-102000,"SYS_EXEC_TAR_ERR"},
            {-103000,"SYS_CANT_CHKSUM_COMP_RESC_DATA"},
            {-104000,"SYS_CANT_CHKSUM_BUNDLED_DATA"},
            {-105000,"SYS_RESC_IS_DOWN"},
            {-106000,"SYS_UPDATE_REPL_INFO_ERR"},
            {-107000,"SYS_COLL_LINK_PATH_ERR"},
            {-108000,"SYS_LINK_CNT_EXCEEDED_ERR"},
            {-109000,"SYS_CROSS_ZONE_MV_NOT_SUPPORTED"},
            {-110000,"SYS_RESC_QUOTA_EXCEEDED"},
            {-111000,"SYS_RENAME_STRUCT_COUNT_EXCEEDED"},
            {-112000,"SYS_BULK_REG_COUNT_EXCEEDED"},
            {-113000,"SYS_REQUESTED_BUF_TOO_LARGE"},
            {-114000,"SYS_INVALID_RESC_FOR_BULK_OPR"},
            {-115000,"SYS_SOCK_READ_TIMEDOUT"},
            {-116000,"SYS_SOCK_READ_ERR"},
            {-117000,"SYS_CONNECT_CONTROL_CONFIG_ERR"},
            {-118000,"SYS_MAX_CONNECT_COUNT_EXCEEDED"},
            {-119000,"SYS_STRUCT_ELEMENT_MISMATCH"},
            {-120000,"SYS_PHY_PATH_INUSE"},
            {-121000,"SYS_USER_NO_PERMISSION"},
            {-122000,"SYS_USER_RETRIEVE_ERR"},
            {-123000,"SYS_FS_LOCK_ERR"},
            {-124000,"SYS_LOCK_TYPE_INP_ERR"},
            {-125000,"SYS_LOCK_CMD_INP_ERR"},
            {-126000,"SYS_ZIP_FORMAT_NOT_SUPPORTED"},
            {-127000,"SYS_ADD_TO_ARCH_OPR_NOT_SUPPORTED"},
            {-128000,"CANT_REG_IN_VAULT_FILE"},
            {-129000,"PATH_REG_NOT_ALLOWED"},
            {-130000,"SYS_INVALID_INPUT_PARAM"},
            {-131000,"SYS_GROUP_RETRIEVE_ERR"},
            {-132000,"SYS_MSSO_APPEND_ERR"},
            {-133000,"SYS_MSSO_STRUCT_FILE_EXTRACT_ERR"},
            {-134000,"SYS_MSSO_EXTRACT_ALL_ERR"},
            {-135000,"SYS_MSSO_OPEN_ERR"},
            {-136000,"SYS_MSSO_CLOSE_ERR"},
            {-144000,"SYS_RULE_NOT_FOUND"},
            {-146000,"SYS_NOT_IMPLEMENTED"},
            {-147000,"SYS_SIGNED_SID_NOT_MATCHED"},
            {-148000,"SYS_HASH_IMMUTABLE"},
            {-149000,"SYS_UNINITIALIZED"},
            {-150000,"SYS_NEGATIVE_SIZE"},
            {-151000,"SYS_ALREADY_INITIALIZED"},
            {-152000,"SYS_SETENV_ERR"},
            {-153000,"SYS_GETENV_ERR"},
            {-154000,"SYS_INTERNAL_ERR"},
            {-155000,"SYS_SOCK_SELECT_ERR"},
            {-156000,"SYS_THREAD_ENCOUNTERED_INTERRUPT"},
            {-157000,"SYS_THREAD_RESOURCE_ERR"},
            {-158000,"SYS_BAD_INPUT"},
            {-159000,"SYS_PORT_RANGE_EXHAUSTED"},
            {-160000,"SYS_SERVICE_ROLE_NOT_SUPPORTED"},
            {-161000,"SYS_SOCK_WRITE_ERR"},
            {-162000,"SYS_SOCK_CONNECT_ERR"},
            {-163000,"SYS_OPERATION_IN_PROGRESS"},
            {-164000,"SYS_REPLICA_DOES_NOT_EXIST"},
            {-165000,"SYS_UNKNOWN_ERROR"},
            {-300000,"USER_AUTH_SCHEME_ERR"},
            {-301000,"USER_AUTH_STRING_EMPTY"},
            {-302000,"USER_RODS_HOST_EMPTY"},
            {-303000,"USER_RODS_HOSTNAME_ERR"},
            {-304000,"USER_SOCK_OPEN_ERR"},
            {-305000,"USER_SOCK_CONNECT_ERR"},
            {-306000,"USER_STRLEN_TOOLONG"},
            {-307000,"USER_API_INPUT_ERR"},
            {-308000,"USER_PACKSTRUCT_INPUT_ERR"},
            {-309000,"USER_NO_SUPPORT_ERR"},
            {-310000,"USER_FILE_DOES_NOT_EXIST"},
            {-311000,"USER_FILE_TOO_LARGE"},
            {-312000,"OVERWRITE_WITHOUT_FORCE_FLAG"},
            {-313000,"UNMATCHED_KEY_OR_INDEX"},
            {-314000,"USER_CHKSUM_MISMATCH"},
            {-315000,"USER_BAD_KEYWORD_ERR"},
            {-316000,"USER__NULL_INPUT_ERR"},
            {-317000,"USER_INPUT_PATH_ERR"},
            {-318000,"USER_INPUT_OPTION_ERR"},
            {-319000,"USER_INVALID_USERNAME_FORMAT"},
            {-320000,"USER_DIRECT_RESC_INPUT_ERR"},
            {-321000,"USER_NO_RESC_INPUT_ERR"},
            {-322000,"USER_PARAM_LABEL_ERR"},
            {-323000,"USER_PARAM_TYPE_ERR"},
            {-324000,"BASE64_BUFFER_OVERFLOW"},
            {-325000,"BASE64_INVALID_PACKET"},
            {-326000,"USER_MSG_TYPE_NO_SUPPORT"},
            {-337000,"USER_RSYNC_NO_MODE_INPUT_ERR"},
            {-338000,"USER_OPTION_INPUT_ERR"},
            {-339000,"SAME_SRC_DEST_PATHS_ERR"},
            {-340000,"USER_RESTART_FILE_INPUT_ERR"},
            {-341000,"RESTART_OPR_FAILED"},
            {-342000,"BAD_EXEC_CMD_PATH"},
            {-343000,"EXEC_CMD_OUTPUT_TOO_LARGE"},
            {-344000,"EXEC_CMD_ERROR"},
            {-345000,"BAD_INPUT_DESC_INDEX"},
            {-346000,"USER_PATH_EXCEEDS_MAX"},
            {-347000,"USER_SOCK_CONNECT_TIMEDOUT"},
            {-348000,"USER_API_VERSION_MISMATCH"},
            {-349000,"USER_INPUT_FORMAT_ERR"},
            {-350000,"USER_ACCESS_DENIED"},
            {-351000,"CANT_RM_MV_BUNDLE_TYPE"},
            {-352000,"NO_MORE_RESULT"},
            {-353000,"NO_KEY_WD_IN_MS_INP_STR"},
            {-354000,"CANT_RM_NON_EMPTY_HOME_COLL"},
            {-355000,"CANT_UNREG_IN_VAULT_FILE"},
            {-356000,"NO_LOCAL_FILE_RSYNC_IN_MSI"},
            {-357000,"BULK_OPR_MISMATCH_FOR_RESTART"},
            {-358000,"OBJ_PATH_DOES_NOT_EXIST"},
            {-359000,"SYMLINKED_BUNFILE_NOT_ALLOWED"},
            {-360000,"USER_INPUT_STRING_ERR"},
            {-361000,"USER_INVALID_RESC_INPUT"},
            {-370000,"USER_NOT_ALLOWED_TO_EXEC_CMD"},
            {-380000,"USER_HASH_TYPE_MISMATCH"},
            {-390000,"USER_INVALID_CLIENT_ENVIRONMENT"},
            {-400000,"USER_INSUFFICIENT_FREE_INODES"},
            {-401000,"USER_FILE_SIZE_MISMATCH"},
            {-402000,"USER_INCOMPATIBLE_PARAMS"},
            {-403000,"USER_INVALID_REPLICA_INPUT"},
            {-500000,"FILE_INDEX_LOOKUP_ERR"},
            {-510000,"UNIX_FILE_OPEN_ERR"},
            {-511000,"UNIX_FILE_CREATE_ERR"},
            {-512000,"UNIX_FILE_READ_ERR"},
            {-513000,"UNIX_FILE_WRITE_ERR"},
            {-514000,"UNIX_FILE_CLOSE_ERR"},
            {-515000,"UNIX_FILE_UNLINK_ERR"},
            {-516000,"UNIX_FILE_STAT_ERR"},
            {-517000,"UNIX_FILE_FSTAT_ERR"},
            {-518000,"UNIX_FILE_LSEEK_ERR"},
            {-519000,"UNIX_FILE_FSYNC_ERR"},
            {-520000,"UNIX_FILE_MKDIR_ERR"},
            {-521000,"UNIX_FILE_RMDIR_ERR"},
            {-522000,"UNIX_FILE_OPENDIR_ERR"},
            {-523000,"UNIX_FILE_CLOSEDIR_ERR"},
            {-524000,"UNIX_FILE_READDIR_ERR"},
            {-525000,"UNIX_FILE_STAGE_ERR"},
            {-526000,"UNIX_FILE_GET_FS_FREESPACE_ERR"},
            {-527000,"UNIX_FILE_CHMOD_ERR"},
            {-528000,"UNIX_FILE_RENAME_ERR"},
            {-529000,"UNIX_FILE_TRUNCATE_ERR"},
            {-530000,"UNIX_FILE_LINK_ERR"},
            {-540000,"UNIX_FILE_OPR_TIMEOUT_ERR"},
            {-550000,"UNIV_MSS_SYNCTOARCH_ERR"},
            {-551000,"UNIV_MSS_STAGETOCACHE_ERR"},
            {-552000,"UNIV_MSS_UNLINK_ERR"},
            {-553000,"UNIV_MSS_MKDIR_ERR"},
            {-554000,"UNIV_MSS_CHMOD_ERR"},
            {-555000,"UNIV_MSS_STAT_ERR"},
            {-556000,"UNIV_MSS_RENAME_ERR"},
            {-600000,"HPSS_AUTH_NOT_SUPPORTED"},
            {-610000,"HPSS_FILE_OPEN_ERR"},
            {-611000,"HPSS_FILE_CREATE_ERR"},
            {-612000,"HPSS_FILE_READ_ERR"},
            {-613000,"HPSS_FILE_WRITE_ERR"},
            {-614000,"HPSS_FILE_CLOSE_ERR"},
            {-615000,"HPSS_FILE_UNLINK_ERR"},
            {-616000,"HPSS_FILE_STAT_ERR"},
            {-617000,"HPSS_FILE_FSTAT_ERR"},
            {-618000,"HPSS_FILE_LSEEK_ERR"},
            {-619000,"HPSS_FILE_FSYNC_ERR"},
            {-620000,"HPSS_FILE_MKDIR_ERR"},
            {-621000,"HPSS_FILE_RMDIR_ERR"},
            {-622000,"HPSS_FILE_OPENDIR_ERR"},
            {-623000,"HPSS_FILE_CLOSEDIR_ERR"},
            {-624000,"HPSS_FILE_READDIR_ERR"},
            {-625000,"HPSS_FILE_STAGE_ERR"},
            {-626000,"HPSS_FILE_GET_FS_FREESPACE_ERR"},
            {-627000,"HPSS_FILE_CHMOD_ERR"},
            {-628000,"HPSS_FILE_RENAME_ERR"},
            {-629000,"HPSS_FILE_TRUNCATE_ERR"},
            {-630000,"HPSS_FILE_LINK_ERR"},
            {-631000,"HPSS_AUTH_ERR"},
            {-632000,"HPSS_WRITE_LIST_ERR"},
            {-633000,"HPSS_READ_LIST_ERR"},
            {-634000,"HPSS_TRANSFER_ERR"},
            {-635000,"HPSS_MOVER_PROT_ERR"},
            {-701000,"S3_INIT_ERROR"},
            {-702000,"S3_PUT_ERROR"},
            {-703000,"S3_GET_ERROR"},
            {-715000,"S3_FILE_UNLINK_ERR"},
            {-716000,"S3_FILE_STAT_ERR"},
            {-717000,"S3_FILE_COPY_ERR"},
            {-718000,"S3_FILE_OPEN_ERR"},
            {-719000,"S3_FILE_SEEK_ERR"},
            {-720000,"S3_FILE_RENAME_ERR"},
            {-750000,"WOS_PUT_ERR"},
            {-751000,"WOS_STREAM_PUT_ERR"},
            {-752000,"WOS_STREAM_CLOSE_ERR"},
            {-753000,"WOS_GET_ERR"},
            {-754000,"WOS_STREAM_GET_ERR"},
            {-755000,"WOS_UNLINK_ERR"},
            {-756000,"WOS_STAT_ERR"},
            {-757000,"WOS_CONNECT_ERR"},
            {-730000,"HDFS_FILE_OPEN_ERR"},
            {-731000,"HDFS_FILE_CREATE_ERR"},
            {-732000,"HDFS_FILE_READ_ERR"},
            {-733000,"HDFS_FILE_WRITE_ERR"},
            {-734000,"HDFS_FILE_CLOSE_ERR"},
            {-735000,"HDFS_FILE_UNLINK_ERR"},
            {-736000,"HDFS_FILE_STAT_ERR"},
            {-737000,"HDFS_FILE_FSTAT_ERR"},
            {-738000,"HDFS_FILE_LSEEK_ERR"},
            {-739000,"HDFS_FILE_FSYNC_ERR"},
            {-741000,"HDFS_FILE_MKDIR_ERR"},
            {-742000,"HDFS_FILE_RMDIR_ERR"},
            {-743000,"HDFS_FILE_OPENDIR_ERR"},
            {-744000,"HDFS_FILE_CLOSEDIR_ERR"},
            {-745000,"HDFS_FILE_READDIR_ERR"},
            {-746000,"HDFS_FILE_STAGE_ERR"},
            {-748000,"HDFS_FILE_CHMOD_ERR"},
            {-749000,"HDFS_FILE_RENAME_ERR"},
            {-760000,"HDFS_FILE_TRUNCATE_ERR"},
            {-761000,"HDFS_FILE_LINK_ERR"},
            {-762000,"HDFS_FILE_OPR_TIMEOUT_ERR"},
            {-770000,"DIRECT_ACCESS_FILE_USER_INVALID_ERR"},
            {-801000,"CATALOG_NOT_CONNECTED"},
            {-802000,"CAT_ENV_ERR"},
            {-803000,"CAT_CONNECT_ERR"},
            {-804000,"CAT_DISCONNECT_ERR"},
            {-805000,"CAT_CLOSE_ENV_ERR"},
            {-806000,"CAT_SQL_ERR"},
            {-807000,"CAT_GET_ROW_ERR"},
            {-808000,"CAT_NO_ROWS_FOUND"},
            {-809000,"CATALOG_ALREADY_HAS_ITEM_BY_THAT_NAME"},
            {-810000,"CAT_INVALID_RESOURCE_TYPE"},
            {-811000,"CAT_INVALID_RESOURCE_CLASS"},
            {-812000,"CAT_INVALID_RESOURCE_NET_ADDR"},
            {-813000,"CAT_INVALID_RESOURCE_VAULT_PATH"},
            {-814000,"CAT_UNKNOWN_COLLECTION"},
            {-815000,"CAT_INVALID_DATA_TYPE"},
            {-816000,"CAT_INVALID_ARGUMENT"},
            {-817000,"CAT_UNKNOWN_FILE"},
            {-818000,"CAT_NO_ACCESS_PERMISSION"},
            {-819000,"CAT_SUCCESS_BUT_WITH_NO_INFO"},
            {-820000,"CAT_INVALID_USER_TYPE"},
            {-821000,"CAT_COLLECTION_NOT_EMPTY"},
            {-822000,"CAT_TOO_MANY_TABLES"},
            {-823000,"CAT_UNKNOWN_TABLE"},
            {-824000,"CAT_NOT_OPEN"},
            {-825000,"CAT_FAILED_TO_LINK_TABLES"},
            {-826000,"CAT_INVALID_AUTHENTICATION"},
            {-827000,"CAT_INVALID_USER"},
            {-828000,"CAT_INVALID_ZONE"},
            {-829000,"CAT_INVALID_GROUP"},
            {-830000,"CAT_INSUFFICIENT_PRIVILEGE_LEVEL"},
            {-831000,"CAT_INVALID_RESOURCE"},
            {-832000,"CAT_INVALID_CLIENT_USER"},
            {-833000,"CAT_NAME_EXISTS_AS_COLLECTION"},
            {-834000,"CAT_NAME_EXISTS_AS_DATAOBJ"},
            {-835000,"CAT_RESOURCE_NOT_EMPTY"},
            {-836000,"CAT_NOT_A_DATAOBJ_AND_NOT_A_COLLECTION"},
            {-837000,"CAT_RECURSIVE_MOVE"},
            {-838000,"CAT_LAST_REPLICA"},
            {-839000,"CAT_OCI_ERROR"},
            {-840000,"CAT_PASSWORD_EXPIRED"},
            {-850000,"CAT_PASSWORD_ENCODING_ERROR"},
            {-851000,"CAT_TABLE_ACCESS_DENIED"},
            {-852000,"CAT_UNKNOWN_RESOURCE"},
            {-853000,"CAT_UNKNOWN_SPECIFIC_QUERY"},
            {-854000,"CAT_PSEUDO_RESC_MODIFY_DISALLOWED"},
            {-855000,"CAT_HOSTNAME_INVALID"},
            {-856000,"CAT_BIND_VARIABLE_LIMIT_EXCEEDED"},
            {-857000,"CAT_INVALID_CHILD"},
            {-858000,"CAT_INVALID_OBJ_COUNT"},
            {-859000,"CAT_INVALID_RESOURCE_NAME"},
            {-860000,"CAT_STATEMENT_TABLE_FULL"},
            {-861000,"CAT_RESOURCE_NAME_LENGTH_EXCEEDED"},
            {-890000,"CAT_TICKET_INVALID"},
            {-891000,"CAT_TICKET_EXPIRED"},
            {-892000,"CAT_TICKET_USES_EXCEEDED"},
            {-893000,"CAT_TICKET_USER_EXCLUDED"},
            {-894000,"CAT_TICKET_HOST_EXCLUDED"},
            {-895000,"CAT_TICKET_GROUP_EXCLUDED"},
            {-896000,"CAT_TICKET_WRITE_USES_EXCEEDED"},
            {-897000,"CAT_TICKET_WRITE_BYTES_EXCEEDED"},
            {-900000,"FILE_OPEN_ERR"},
            {-901000,"FILE_READ_ERR"},
            {-902000,"FILE_WRITE_ERR"},
            {-903000,"PASSWORD_EXCEEDS_MAX_SIZE"},
            {-904000,"ENVIRONMENT_VAR_HOME_NOT_DEFINED"},
            {-905000,"UNABLE_TO_STAT_FILE"},
            {-906000,"AUTH_FILE_NOT_ENCRYPTED"},
            {-907000,"AUTH_FILE_DOES_NOT_EXIST"},
            {-908000,"UNLINK_FAILED"},
            {-909000,"NO_PASSWORD_ENTERED"},
            {-910000,"REMOTE_SERVER_AUTHENTICATION_FAILURE"},
            {-911000,"REMOTE_SERVER_AUTH_NOT_PROVIDED"},
            {-912000,"REMOTE_SERVER_AUTH_EMPTY"},
            {-913000,"REMOTE_SERVER_SID_NOT_DEFINED"},
            {-921000,"GSI_NOT_COMPILED_IN"},
            {-922000,"GSI_NOT_BUILT_INTO_CLIENT"},
            {-923000,"GSI_NOT_BUILT_INTO_SERVER"},
            {-924000,"GSI_ERROR_IMPORT_NAME"},
            {-925000,"GSI_ERROR_INIT_SECURITY_CONTEXT"},
            {-926000,"GSI_ERROR_SENDING_TOKEN_LENGTH"},
            {-927000,"GSI_ERROR_READING_TOKEN_LENGTH"},
            {-928000,"GSI_ERROR_TOKEN_TOO_LARGE"},
            {-929000,"GSI_ERROR_BAD_TOKEN_RCVED"},
            {-930000,"GSI_SOCKET_READ_ERROR"},
            {-931000,"GSI_PARTIAL_TOKEN_READ"},
            {-932000,"GSI_SOCKET_WRITE_ERROR"},
            {-933000,"GSI_ERROR_FROM_GSI_LIBRARY"},
            {-934000,"GSI_ERROR_IMPORTING_NAME"},
            {-935000,"GSI_ERROR_ACQUIRING_CREDS"},
            {-936000,"GSI_ACCEPT_SEC_CONTEXT_ERROR"},
            {-937000,"GSI_ERROR_DISPLAYING_NAME"},
            {-938000,"GSI_ERROR_RELEASING_NAME"},
            {-939000,"GSI_DN_DOES_NOT_MATCH_USER"},
            {-940000,"GSI_QUERY_INTERNAL_ERROR"},
            {-941000,"GSI_NO_MATCHING_DN_FOUND"},
            {-942000,"GSI_MULTIPLE_MATCHING_DN_FOUND"},
            {-951000,"KRB_NOT_COMPILED_IN"},
            {-952000,"KRB_NOT_BUILT_INTO_CLIENT"},
            {-953000,"KRB_NOT_BUILT_INTO_SERVER"},
            {-954000,"KRB_ERROR_IMPORT_NAME"},
            {-955000,"KRB_ERROR_INIT_SECURITY_CONTEXT"},
            {-956000,"KRB_ERROR_SENDING_TOKEN_LENGTH"},
            {-957000,"KRB_ERROR_READING_TOKEN_LENGTH"},
            {-958000,"KRB_ERROR_TOKEN_TOO_LARGE"},
            {-959000,"KRB_ERROR_BAD_TOKEN_RCVED"},
            {-960000,"KRB_SOCKET_READ_ERROR"},
            {-961000,"KRB_PARTIAL_TOKEN_READ"},
            {-962000,"KRB_SOCKET_WRITE_ERROR"},
            {-963000,"KRB_ERROR_FROM_KRB_LIBRARY"},
            {-964000,"KRB_ERROR_IMPORTING_NAME"},
            {-965000,"KRB_ERROR_ACQUIRING_CREDS"},
            {-966000,"KRB_ACCEPT_SEC_CONTEXT_ERROR"},
            {-967000,"KRB_ERROR_DISPLAYING_NAME"},
            {-968000,"KRB_ERROR_RELEASING_NAME"},
            {-969000,"KRB_USER_DN_NOT_FOUND"},
            {-970000,"KRB_NAME_MATCHES_MULTIPLE_USERS"},
            {-971000,"KRB_QUERY_INTERNAL_ERROR"},
            {-981000,"OSAUTH_NOT_BUILT_INTO_CLIENT"},
            {-982000,"OSAUTH_NOT_BUILT_INTO_SERVER"},
            {-991000,"PAM_AUTH_NOT_BUILT_INTO_CLIENT"},
            {-992000,"PAM_AUTH_NOT_BUILT_INTO_SERVER"},
            {-993000,"PAM_AUTH_PASSWORD_FAILED"},
            {-994000,"PAM_AUTH_PASSWORD_INVALID_TTL"},
            {-1000000,"OBJPATH_EMPTY_IN_STRUCT_ERR"},
            {-1001000,"RESCNAME_EMPTY_IN_STRUCT_ERR"},
            {-1002000,"DATATYPE_EMPTY_IN_STRUCT_ERR"},
            {-1003000,"DATASIZE_EMPTY_IN_STRUCT_ERR"},
            {-1004000,"CHKSUM_EMPTY_IN_STRUCT_ERR"},
            {-1005000,"VERSION_EMPTY_IN_STRUCT_ERR"},
            {-1006000,"FILEPATH_EMPTY_IN_STRUCT_ERR"},
            {-1007000,"REPLNUM_EMPTY_IN_STRUCT_ERR"},
            {-1008000,"REPLSTATUS_EMPTY_IN_STRUCT_ERR"},
            {-1009000,"DATAOWNER_EMPTY_IN_STRUCT_ERR"},
            {-1010000,"DATAOWNERZONE_EMPTY_IN_STRUCT_ERR"},
            {-1011000,"DATAEXPIRY_EMPTY_IN_STRUCT_ERR"},
            {-1012000,"DATACOMMENTS_EMPTY_IN_STRUCT_ERR"},
            {-1013000,"DATACREATE_EMPTY_IN_STRUCT_ERR"},
            {-1014000,"DATAMODIFY_EMPTY_IN_STRUCT_ERR"},
            {-1015000,"DATAACCESS_EMPTY_IN_STRUCT_ERR"},
            {-1016000,"DATAACCESSINX_EMPTY_IN_STRUCT_ERR"},
            {-1017000,"NO_RULE_FOUND_ERR"},
            {-1018000,"NO_MORE_RULES_ERR"},
            {-1019000,"UNMATCHED_ACTION_ERR"},
            {-1020000,"RULES_FILE_READ_ERROR"},
            {-1021000,"ACTION_ARG_COUNT_MISMATCH"},
            {-1022000,"MAX_NUM_OF_ARGS_IN_ACTION_EXCEEDED"},
            {-1023000,"UNKNOWN_PARAM_IN_RULE_ERR"},
            {-1024000,"DESTRESCNAME_EMPTY_IN_STRUCT_ERR"},
            {-1025000,"BACKUPRESCNAME_EMPTY_IN_STRUCT_ERR"},
            {-1026000,"DATAID_EMPTY_IN_STRUCT_ERR"},
            {-1027000,"COLLID_EMPTY_IN_STRUCT_ERR"},
            {-1028000,"RESCGROUPNAME_EMPTY_IN_STRUCT_ERR"},
            {-1029000,"STATUSSTRING_EMPTY_IN_STRUCT_ERR"},
            {-1030000,"DATAMAPID_EMPTY_IN_STRUCT_ERR"},
            {-1031000,"USERNAMECLIENT_EMPTY_IN_STRUCT_ERR"},
            {-1032000,"RODSZONECLIENT_EMPTY_IN_STRUCT_ERR"},
            {-1033000,"USERTYPECLIENT_EMPTY_IN_STRUCT_ERR"},
            {-1034000,"HOSTCLIENT_EMPTY_IN_STRUCT_ERR"},
            {-1035000,"AUTHSTRCLIENT_EMPTY_IN_STRUCT_ERR"},
            {-1036000,"USERAUTHSCHEMECLIENT_EMPTY_IN_STRUCT_ERR"},
            {-1037000,"USERINFOCLIENT_EMPTY_IN_STRUCT_ERR"},
            {-1038000,"USERCOMMENTCLIENT_EMPTY_IN_STRUCT_ERR"},
            {-1039000,"USERCREATECLIENT_EMPTY_IN_STRUCT_ERR"},
            {-1040000,"USERMODIFYCLIENT_EMPTY_IN_STRUCT_ERR"},
            {-1041000,"USERNAMEPROXY_EMPTY_IN_STRUCT_ERR"},
            {-1042000,"RODSZONEPROXY_EMPTY_IN_STRUCT_ERR"},
            {-1043000,"USERTYPEPROXY_EMPTY_IN_STRUCT_ERR"},
            {-1044000,"HOSTPROXY_EMPTY_IN_STRUCT_ERR"},
            {-1045000,"AUTHSTRPROXY_EMPTY_IN_STRUCT_ERR"},
            {-1046000,"USERAUTHSCHEMEPROXY_EMPTY_IN_STRUCT_ERR"},
            {-1047000,"USERINFOPROXY_EMPTY_IN_STRUCT_ERR"},
            {-1048000,"USERCOMMENTPROXY_EMPTY_IN_STRUCT_ERR"},
            {-1049000,"USERCREATEPROXY_EMPTY_IN_STRUCT_ERR"},
            {-1050000,"USERMODIFYPROXY_EMPTY_IN_STRUCT_ERR"},
            {-1051000,"COLLNAME_EMPTY_IN_STRUCT_ERR"},
            {-1052000,"COLLPARENTNAME_EMPTY_IN_STRUCT_ERR"},
            {-1053000,"COLLOWNERNAME_EMPTY_IN_STRUCT_ERR"},
            {-1054000,"COLLOWNERZONE_EMPTY_IN_STRUCT_ERR"},
            {-1055000,"COLLEXPIRY_EMPTY_IN_STRUCT_ERR"},
            {-1056000,"COLLCOMMENTS_EMPTY_IN_STRUCT_ERR"},
            {-1057000,"COLLCREATE_EMPTY_IN_STRUCT_ERR"},
            {-1058000,"COLLMODIFY_EMPTY_IN_STRUCT_ERR"},
            {-1059000,"COLLACCESS_EMPTY_IN_STRUCT_ERR"},
            {-1060000,"COLLACCESSINX_EMPTY_IN_STRUCT_ERR"},
            {-1062000,"COLLMAPID_EMPTY_IN_STRUCT_ERR"},
            {-1063000,"COLLINHERITANCE_EMPTY_IN_STRUCT_ERR"},
            {-1065000,"RESCZONE_EMPTY_IN_STRUCT_ERR"},
            {-1066000,"RESCLOC_EMPTY_IN_STRUCT_ERR"},
            {-1067000,"RESCTYPE_EMPTY_IN_STRUCT_ERR"},
            {-1068000,"RESCTYPEINX_EMPTY_IN_STRUCT_ERR"},
            {-1069000,"RESCCLASS_EMPTY_IN_STRUCT_ERR"},
            {-1070000,"RESCCLASSINX_EMPTY_IN_STRUCT_ERR"},
            {-1071000,"RESCVAULTPATH_EMPTY_IN_STRUCT_ERR"},
            {-1072000,"NUMOPEN_ORTS_EMPTY_IN_STRUCT_ERR"},
            {-1073000,"PARAOPR_EMPTY_IN_STRUCT_ERR"},
            {-1074000,"RESCID_EMPTY_IN_STRUCT_ERR"},
            {-1075000,"GATEWAYADDR_EMPTY_IN_STRUCT_ERR"},
            {-1076000,"RESCMAX_BJSIZE_EMPTY_IN_STRUCT_ERR"},
            {-1077000,"FREESPACE_EMPTY_IN_STRUCT_ERR"},
            {-1078000,"FREESPACETIME_EMPTY_IN_STRUCT_ERR"},
            {-1079000,"FREESPACETIMESTAMP_EMPTY_IN_STRUCT_ERR"},
            {-1080000,"RESCINFO_EMPTY_IN_STRUCT_ERR"},
            {-1081000,"RESCCOMMENTS_EMPTY_IN_STRUCT_ERR"},
            {-1082000,"RESCCREATE_EMPTY_IN_STRUCT_ERR"},
            {-1083000,"RESCMODIFY_EMPTY_IN_STRUCT_ERR"},
            {-1084000,"INPUT_ARG_NOT_WELL_FORMED_ERR"},
            {-1085000,"INPUT_ARG_OUT_OF_ARGC_RANGE_ERR"},
            {-1086000,"INSUFFICIENT_INPUT_ARG_ERR"},
            {-1087000,"INPUT_ARG_DOES_NOT_MATCH_ERR"},
            {-1088000,"RETRY_WITHOUT_RECOVERY_ERR"},
            {-1089000,"CUT_ACTION_PROCESSED_ERR"},
            {-1090000,"ACTION_FAILED_ERR"},
            {-1091000,"FAIL_ACTION_ENCOUNTERED_ERR"},
            {-1092000,"VARIABLE_NAME_TOO_LONG_ERR"},
            {-1093000,"UNKNOWN_VARIABLE_MAP_ERR"},
            {-1094000,"UNDEFINED_VARIABLE_MAP_ERR"},
            {-1095000,"NULL_VALUE_ERR"},
            {-1096000,"DVARMAP_FILE_READ_ERROR"},
            {-1097000,"NO_RULE_OR_MSI_FUNCTION_FOUND_ERR"},
            {-1098000,"FILE_CREATE_ERROR"},
            {-1099000,"FMAP_FILE_READ_ERROR"},
            {-1100000,"DATE_FORMAT_ERR"},
            {-1101000,"RULE_FAILED_ERR"},
            {-1102000,"NO_MICROSERVICE_FOUND_ERR"},
            {-1103000,"INVALID_REGEXP"},
            {-1104000,"INVALID_OBJECT_NAME"},
            {-1105000,"INVALID_OBJECT_TYPE"},
            {-1106000,"NO_VALUES_FOUND"},
            {-1107000,"NO_COLUMN_NAME_FOUND"},
            {-1108000,"BREAK_ACTION_ENCOUNTERED_ERR"},
            {-1109000,"CUT_ACTION_ON_SUCCESS_PROCESSED_ERR"},
            {-1110000,"MSI_OPERATION_NOT_ALLOWED"},
            {-1111000,"MAX_NUM_OF_ACTION_IN_RULE_EXCEEDED"},
            {-1112000,"MSRVC_FILE_READ_ERROR"},
            {-1113000,"MSRVC_VERSION_MISMATCH"},
            {-1114000,"MICRO_SERVICE_OBJECT_TYPE_UNDEFINED"},
            {-1115000,"MSO_OBJ_GET_FAILED"},
            {-1116000,"REMOTE_IRODS_CONNECT_ERR"},
            {-1117000,"REMOTE_SRB_CONNECT_ERR"},
            {-1118000,"MSO_OBJ_PUT_FAILED"},
            {-1201000,"RE_PARSER_ERROR"},
            {-1202000,"RE_UNPARSED_SUFFIX"},
            {-1203000,"RE_POINTER_ERROR"},
            {-1205000,"RE_RUNTIME_ERROR"},
            {-1206000,"RE_DIVISION_BY_ZERO"},
            {-1207000,"RE_BUFFER_OVERFLOW"},
            {-1208000,"RE_UNSUPPORTED_OP_OR_TYPE"},
            {-1209000,"RE_UNSUPPORTED_SESSION_VAR"},
            {-1210000,"RE_UNABLE_TO_WRITE_LOCAL_VAR"},
            {-1211000,"RE_UNABLE_TO_READ_LOCAL_VAR"},
            {-1212000,"RE_UNABLE_TO_WRITE_SESSION_VAR"},
            {-1213000,"RE_UNABLE_TO_READ_SESSION_VAR"},
            {-1214000,"RE_UNABLE_TO_WRITE_VAR"},
            {-1215000,"RE_UNABLE_TO_READ_VAR"},
            {-1216000,"RE_PATTERN_NOT_MATCHED"},
            {-1217000,"RE_STRING_OVERFLOW"},
            {-1220000,"RE_UNKNOWN_ERROR"},
            {-1221000,"RE_OUT_OF_MEMORY"},
            {-1222000,"RE_SHM_UNLINK_ERROR"},
            {-1223000,"RE_FILE_STAT_ERROR"},
            {-1224000,"RE_UNSUPPORTED_AST_NODE_TYPE"},
            {-1225000,"RE_UNSUPPORTED_SESSION_VAR_TYPE"},
            {-1230000,"RE_TYPE_ERROR"},
            {-1231000,"RE_FUNCTION_REDEFINITION"},
            {-1232000,"RE_DYNAMIC_TYPE_ERROR"},
            {-1233000,"RE_DYNAMIC_COERCION_ERROR"},
            {-1234000,"RE_PACKING_ERROR"},
            {-1600000,"PHP_EXEC_SCRIPT_ERR"},
            {-1601000,"PHP_REQUEST_STARTUP_ERR"},
            {-1602000,"PHP_OPEN_SCRIPT_FILE_ERR"},
            {-1800000,"KEY_NOT_FOUND"},
            {-1801000,"KEY_TYPE_MISMATCH"},
            {-1802000,"CHILD_EXISTS"},
            {-1803000,"HIERARCHY_ERROR"},
            {-1804000,"CHILD_NOT_FOUND"},
            {-1805000,"NO_NEXT_RESC_FOUND"},
            {-1806000,"NO_PDMO_DEFINED"},
            {-1807000,"INVALID_LOCATION"},
            {-1808000,"PLUGIN_ERROR"},
            {-1809000,"INVALID_RESC_CHILD_CONTEXT"},
            {-1810000,"INVALID_FILE_OBJECT"},
            {-1811000,"INVALID_OPERATION"},
            {-1812000,"CHILD_HAS_PARENT"},
            {-1813000,"FILE_NOT_IN_VAULT"},
            {-1814000,"DIRECT_ARCHIVE_ACCESS"},
            {-1815000,"ADVANCED_NEGOTIATION_NOT_SUPPORTED"},
            {-1816000,"DIRECT_CHILD_ACCESS"},
            {-1817000,"INVALID_DYNAMIC_CAST"},
            {-1818000,"INVALID_ACCESS_TO_IMPOSTOR_RESOURCE"},
            {-1819000,"INVALID_LEXICAL_CAST"},
            {-1820000,"CONTROL_PLANE_MESSAGE_ERROR"},
            {-1821000,"REPLICA_NOT_IN_RESC"},
            {-1822000,"INVALID_ANY_CAST"},
            {-1823000,"BAD_FUNCTION_CALL"},
            {-1824000,"CLIENT_NEGOTIATION_ERROR"},
            {-1825000,"SERVER_NEGOTIATION_ERROR"},
            {-1826000,"INVALID_KVP_STRING"},
            {-1827000,"PLUGIN_ERROR_MISSING_SHARED_OBJECT"},
            {-1828000,"RULE_ENGINE_ERROR"},
            {-1829000,"REBALANCE_ALREADY_ACTIVE_ON_RESOURCE"},
            {-2000000,"NETCDF_OPEN_ERR"},
            {-2001000,"NETCDF_CREATE_ERR"},
            {-2002000,"NETCDF_CLOSE_ERR"},
            {-2003000,"NETCDF_INVALID_PARAM_TYPE"},
            {-2004000,"NETCDF_INQ_ID_ERR"},
            {-2005000,"NETCDF_GET_VARS_ERR"},
            {-2006000,"NETCDF_INVALID_DATA_TYPE"},
            {-2007000,"NETCDF_INQ_VARS_ERR"},
            {-2008000,"NETCDF_VARS_DATA_TOO_BIG"},
            {-2009000,"NETCDF_DIM_MISMATCH_ERR"},
            {-2010000,"NETCDF_INQ_ERR"},
            {-2011000,"NETCDF_INQ_FORMAT_ERR"},
            {-2012000,"NETCDF_INQ_DIM_ERR"},
            {-2013000,"NETCDF_INQ_ATT_ERR"},
            {-2014000,"NETCDF_GET_ATT_ERR"},
            {-2015000,"NETCDF_VAR_COUNT_OUT_OF_RANGE"},
            {-2016000,"NETCDF_UNMATCHED_NAME_ERR"},
            {-2017000,"NETCDF_NO_UNLIMITED_DIM"},
            {-2018000,"NETCDF_PUT_ATT_ERR"},
            {-2019000,"NETCDF_DEF_DIM_ERR"},
            {-2020000,"NETCDF_DEF_VAR_ERR"},
            {-2021000,"NETCDF_PUT_VARS_ERR"},
            {-2022000,"NETCDF_AGG_INFO_FILE_ERR"},
            {-2023000,"NETCDF_AGG_ELE_INX_OUT_OF_RANGE"},
            {-2024000,"NETCDF_AGG_ELE_FILE_NOT_OPENED"},
            {-2025000,"NETCDF_AGG_ELE_FILE_NO_TIME_DIM"},
            {-2100000,"SSL_NOT_BUILT_INTO_CLIENT"},
            {-2101000,"SSL_NOT_BUILT_INTO_SERVER"},
            {-2102000,"SSL_INIT_ERROR"},
            {-2103000,"SSL_HANDSHAKE_ERROR"},
            {-2104000,"SSL_SHUTDOWN_ERROR"},
            {-2105000,"SSL_CERT_ERROR"},
            {-2200000,"OOI_CURL_EASY_INIT_ERR"},
            {-2201000,"OOI_JSON_OBJ_SET_ERR"},
            {-2202000,"OOI_DICT_TYPE_NOT_SUPPORTED"},
            {-2203000,"OOI_JSON_PACK_ERR"},
            {-2204000,"OOI_JSON_DUMP_ERR"},
            {-2205000,"OOI_CURL_EASY_PERFORM_ERR"},
            {-2206000,"OOI_JSON_LOAD_ERR"},
            {-2207000,"OOI_JSON_GET_ERR"},
            {-2208000,"OOI_JSON_NO_ANSWER_ERR"},
            {-2209000,"OOI_JSON_TYPE_ERR"},
            {-2210000,"OOI_JSON_INX_OUT_OF_RANGE"},
            {-2211000,"OOI_REVID_NOT_FOUND"},
            {-3000000,"DEPRECATED_PARAMETER"},
            {-2300000,"XML_PARSING_ERR"},
            {-2301000,"OUT_OF_URL_PATH"},
            {-2302000,"URL_PATH_INX_OUT_OF_RANGE"},
            {-99999996,"SYS_NULL_INPUT"},
            {-99999997,"SYS_HANDLER_DONE_WITH_ERROR"},
            {-99999998,"SYS_HANDLER_DONE_NO_ERROR"},
            {-99999999,"SYS_NO_HANDLER_REPLY_MSG"},
        };
    }
    public static class ICATAttributes
    {
        public const string ZONE_ID = "ZONE_ID";
        public const string ZONE_NAME = "ZONE_NAME";
        public const string ZONE_TYPE = "ZONE_TYPE";
        public const string ZONE_CONNECTION = "ZONE_CONNECTION";
        public const string ZONE_COMMENT = "ZONE_COMMENT";
        public const string ZONE_CREATE_TIME = "ZONE_CREATE_TIME";
        public const string ZONE_MODIFY_TIME = "ZONE_MODIFY_TIME";
        public const string USER_ID = "USER_ID";
        public const string USER_NAME = "USER_NAME";
        public const string USER_TYPE = "USER_TYPE";
        public const string USER_ZONE = "USER_ZONE";
        public const string USER_DN = "USER_DN";
        public const string USER_INFO = "USER_INFO";
        public const string USER_COMMENT = "USER_COMMENT";
        public const string USER_CREATE_TIME = "USER_CREATE_TIME";
        public const string USER_MODIFY_TIME = "USER_MODIFY_TIME";
        public const string RESC_ID = "RESC_ID";
        public const string RESC_NAME = "RESC_NAME";
        public const string RESC_ZONE_NAME = "RESC_ZONE_NAME";
        public const string RESC_TYPE_NAME = "RESC_TYPE_NAME";
        public const string RESC_CLASS_NAME = "RESC_CLASS_NAME";
        public const string RESC_LOC = "RESC_LOC";
        public const string RESC_VAULT_PATH = "RESC_VAULT_PATH";
        public const string RESC_FREE_SPACE = "RESC_FREE_SPACE";
        public const string RESC_FREE_SPACE_TIME = "RESC_FREE_SPACE_TIME";
        public const string RESC_INFO = "RESC_INFO";
        public const string RESC_COMMENT = "RESC_COMMENT";
        public const string RESC_CREATE_TIME = "RESC_CREATE_TIME";
        public const string RESC_MODIFY_TIME = "RESC_MODIFY_TIME";
        public const string RESC_STATUS = "RESC_STATUS";
        public const string RESC_CHILDREN = "RESC_CHILDREN";
        public const string RESC_CONTEXT = "RESC_CONTEXT";
        public const string RESC_PARENT = "RESC_PARENT";
        public const string RESC_PARENT_CONTEXT = "RESC_PARENT_CONTEXT";
        public const string DATA_ID = "DATA_ID";
        public const string DATA_COLL_ID = "DATA_COLL_ID";
        public const string DATA_NAME = "DATA_NAME";
        public const string DATA_REPL_NUM = "DATA_REPL_NUM";
        public const string DATA_VERSION = "DATA_VERSION";
        public const string DATA_TYPE_NAME = "DATA_TYPE_NAME";
        public const string DATA_SIZE = "DATA_SIZE";
        public const string DATA_RESC_NAME = "DATA_RESC_NAME";
        public const string DATA_RESC_HIER = "DATA_RESC_HIER";
        public const string DATA_PATH = "DATA_PATH";
        public const string DATA_OWNER_NAME = "DATA_OWNER_NAME";
        public const string DATA_OWNER_ZONE = "DATA_OWNER_ZONE";
        public const string DATA_REPL_STATUS = "DATA_REPL_STATUS";
        public const string DATA_STATUS = "DATA_STATUS";
        public const string DATA_CHECKSUM = "DATA_CHECKSUM";
        public const string DATA_EXPIRY = "DATA_EXPIRY";
        public const string DATA_MAP_ID = "DATA_MAP_ID";
        public const string DATA_COMMENTS = "DATA_COMMENTS";
        public const string DATA_CREATE_TIME = "DATA_CREATE_TIME";
        public const string DATA_MODIFY_TIME = "DATA_MODIFY_TIME";
        public const string DATA_RESC_ID = "DATA_RESC_ID";
        public const string DATA_ACCESS_TYPE = "DATA_ACCESS_TYPE";
        public const string DATA_ACCESS_NAME = "DATA_ACCESS_NAME";
        public const string DATA_TOKEN_NAMESPACE = "DATA_TOKEN_NAMESPACE";
        public const string DATA_ACCESS_USER_ID = "DATA_ACCESS_USER_ID";
        public const string DATA_ACCESS_DATA_ID = "DATA_ACCESS_DATA_ID";
        public const string COLL_ID = "COLL_ID";
        public const string COLL_NAME = "COLL_NAME";
        public const string COLL_PARENT_NAME = "COLL_PARENT_NAME";
        public const string COLL_OWNER_NAME = "COLL_OWNER_NAME";
        public const string COLL_OWNER_ZONE = "COLL_OWNER_ZONE";
        public const string COLL_MAP_ID = "COLL_MAP_ID";
        public const string COLL_INHERITANCE = "COLL_INHERITANCE";
        public const string COLL_COMMENTS = "COLL_COMMENTS";
        public const string COLL_CREATE_TIME = "COLL_CREATE_TIME";
        public const string COLL_MODIFY_TIME = "COLL_MODIFY_TIME";
        public const string COLL_ACCESS_TYPE = "COLL_ACCESS_TYPE";
        public const string COLL_ACCESS_NAME = "COLL_ACCESS_NAME";
        public const string COLL_TOKEN_NAMESPACE = "COLL_TOKEN_NAMESPACE";
        public const string COLL_ACCESS_USER_ID = "COLL_ACCESS_USER_ID";
        public const string COLL_ACCESS_COLL_ID = "COLL_ACCESS_COLL_ID";
        public const string META_DATA_ATTR_NAME = "META_DATA_ATTR_NAME";
        public const string META_DATA_ATTR_VALUE = "META_DATA_ATTR_VALUE";
        public const string META_DATA_ATTR_UNITS = "META_DATA_ATTR_UNITS";
        public const string META_DATA_ATTR_ID = "META_DATA_ATTR_ID";
        public const string META_DATA_CREATE_TIME = "META_DATA_CREATE_TIME";
        public const string META_DATA_MODIFY_TIME = "META_DATA_MODIFY_TIME";
        public const string META_COLL_ATTR_NAME = "META_COLL_ATTR_NAME";
        public const string META_COLL_ATTR_VALUE = "META_COLL_ATTR_VALUE";
        public const string META_COLL_ATTR_UNITS = "META_COLL_ATTR_UNITS";
        public const string META_COLL_ATTR_ID = "META_COLL_ATTR_ID";
        public const string META_COLL_CREATE_TIME = "META_COLL_CREATE_TIME";
        public const string META_COLL_MODIFY_TIME = "META_COLL_MODIFY_TIME";
        public const string META_NAMESPACE_COLL = "META_NAMESPACE_COLL";
        public const string META_NAMESPACE_DATA = "META_NAMESPACE_DATA";
        public const string META_NAMESPACE_RESC = "META_NAMESPACE_RESC";
        public const string META_NAMESPACE_USER = "META_NAMESPACE_USER";
        public const string META_NAMESPACE_RESC_GROUP = "META_NAMESPACE_RESC_GROUP";
        public const string META_NAMESPACE_RULE = "META_NAMESPACE_RULE";
        public const string META_NAMESPACE_MSRVC = "META_NAMESPACE_MSRVC";
        public const string META_NAMESPACE_MET2 = "META_NAMESPACE_MET2";
        public const string META_RESC_ATTR_NAME = "META_RESC_ATTR_NAME";
        public const string META_RESC_ATTR_VALUE = "META_RESC_ATTR_VALUE";
        public const string META_RESC_ATTR_UNITS = "META_RESC_ATTR_UNITS";
        public const string META_RESC_ATTR_ID = "META_RESC_ATTR_ID";
        public const string META_RESC_CREATE_TIME = "META_RESC_CREATE_TIME";
        public const string META_RESC_MODIFY_TIME = "META_RESC_MODIFY_TIME";
        public const string META_RESC_GROUP_ATTR_NAME = "META_RESC_GROUP_ATTR_NAME";
        public const string META_RESC_GROUP_ATTR_VALUE = "META_RESC_GROUP_ATTR_VALUE";
        public const string META_RESC_GROUP_ATTR_UNITS = "META_RESC_GROUP_ATTR_UNITS";
        public const string META_RESC_GROUP_ATTR_ID = "META_RESC_GROUP_ATTR_ID";
        public const string META_RESC_GROUP_CREATE_TIME = "META_RESC_GROUP_CREATE_TIME";
        public const string META_RESC_GROUP_MODIFY_TIME = "META_RESC_GROUP_MODIFY_TIME";
        public const string META_USER_ATTR_NAME = "META_USER_ATTR_NAME";
        public const string META_USER_ATTR_VALUE = "META_USER_ATTR_VALUE";
        public const string META_USER_ATTR_UNITS = "META_USER_ATTR_UNITS";
        public const string META_USER_ATTR_ID = "META_USER_ATTR_ID";
        public const string META_USER_CREATE_TIME = "META_USER_CREATE_TIME";
        public const string META_USER_MODIFY_TIME = "META_USER_MODIFY_TIME";
        public const string META_RULE_ATTR_NAME = "META_RULE_ATTR_NAME";
        public const string META_RULE_ATTR_VALUE = "META_RULE_ATTR_VALUE";
        public const string META_RULE_ATTR_UNITS = "META_RULE_ATTR_UNITS";
        public const string META_RULE_ATTR_ID = "META_RULE_ATTR_ID";
        public const string META_RULE_CREATE_TIME = "META_RULE_CREATE_TIME";
        public const string META_RULE_MODIFY_TIME = "META_RULE_MODIFY_TIME";
        public const string META_MSRVC_ATTR_NAME = "META_MSRVC_ATTR_NAME";
        public const string META_MSRVC_ATTR_VALUE = "META_MSRVC_ATTR_VALUE";
        public const string META_MSRVC_ATTR_UNITS = "META_MSRVC_ATTR_UNITS";
        public const string META_MSRVC_ATTR_ID = "META_MSRVC_ATTR_ID";
        public const string META_MSRVC_CREATE_TIME = "META_MSRVC_CREATE_TIME";
        public const string META_MSRVC_MODIFY_TIME = "META_MSRVC_MODIFY_TIME";
        public const string META_MET2_ATTR_NAME = "META_MET2_ATTR_NAME";
        public const string META_MET2_ATTR_VALUE = "META_MET2_ATTR_VALUE";
        public const string META_MET2_ATTR_UNITS = "META_MET2_ATTR_UNITS";
        public const string META_MET2_ATTR_ID = "META_MET2_ATTR_ID";
        public const string META_MET2_CREATE_TIME = "META_MET2_CREATE_TIME";
        public const string META_MET2_MODIFY_TIME = "META_MET2_MODIFY_TIME";
        public const string USER_GROUP_ID = "USER_GROUP_ID";
        public const string USER_GROUP_NAME = "USER_GROUP_NAME";
        public const string RULE_EXEC_ID = "RULE_EXEC_ID";
        public const string RULE_EXEC_NAME = "RULE_EXEC_NAME";
        public const string RULE_EXEC_REI_FILE_PATH = "RULE_EXEC_REI_FILE_PATH";
        public const string RULE_EXEC_USER_NAME = "RULE_EXEC_USER_NAME";
        public const string RULE_EXEC_ADDRESS = "RULE_EXEC_ADDRESS";
        public const string RULE_EXEC_TIME = "RULE_EXEC_TIME";
        public const string RULE_EXEC_FREQUENCY = "RULE_EXEC_FREQUENCY";
        public const string RULE_EXEC_PRIORITY = "RULE_EXEC_PRIORITY";
        public const string RULE_EXEC_ESTIMATED_EXE_TIME = "RULE_EXEC_ESTIMATED_EXE_TIME";
        public const string RULE_EXEC_NOTIFICATION_ADDR = "RULE_EXEC_NOTIFICATION_ADDR";
        public const string RULE_EXEC_LAST_EXE_TIME = "RULE_EXEC_LAST_EXE_TIME";
        public const string RULE_EXEC_STATUS = "RULE_EXEC_STATUS";
        public const string TOKEN_NAMESPACE = "TOKEN_NAMESPACE";
        public const string TOKEN_ID = "TOKEN_ID";
        public const string TOKEN_NAME = "TOKEN_NAME";
        public const string TOKEN_VALUE = "TOKEN_VALUE";
        public const string TOKEN_VALUE2 = "TOKEN_VALUE2";
        public const string TOKEN_VALUE3 = "TOKEN_VALUE3";
        public const string TOKEN_COMMENT = "TOKEN_COMMENT";
        public const string AUDIT_OBJ_ID = "AUDIT_OBJ_ID";
        public const string AUDIT_USER_ID = "AUDIT_USER_ID";
        public const string AUDIT_ACTION_ID = "AUDIT_ACTION_ID";
        public const string AUDIT_COMMENT = "AUDIT_COMMENT";
        public const string AUDIT_CREATE_TIME = "AUDIT_CREATE_TIME";
        public const string AUDIT_MODIFY_TIME = "AUDIT_MODIFY_TIME";
        public const string SL_HOST_NAME = "SL_HOST_NAME";
        public const string SL_RESC_NAME = "SL_RESC_NAME";
        public const string SL_CPU_USED = "SL_CPU_USED";
        public const string SL_MEM_USED = "SL_MEM_USED";
        public const string SL_SWAP_USED = "SL_SWAP_USED";
        public const string SL_RUNQ_LOAD = "SL_RUNQ_LOAD";
        public const string SL_DISK_SPACE = "SL_DISK_SPACE";
        public const string SL_NET_INPUT = "SL_NET_INPUT";
        public const string SL_NET_OUTPUT = "SL_NET_OUTPUT";
        public const string SL_CREATE_TIME = "SL_CREATE_TIME";
        public const string SLD_RESC_NAME = "SLD_RESC_NAME";
        public const string SLD_LOAD_FACTOR = "SLD_LOAD_FACTOR";
        public const string SLD_CREATE_TIME = "SLD_CREATE_TIME";
        public const string RULE_BASE_MAP_VERSION = "RULE_BASE_MAP_VERSION";
        public const string RULE_BASE_MAP_PRIORITY = "RULE_BASE_MAP_PRIORITY";
        public const string RULE_BASE_MAP_BASE_NAME = "RULE_BASE_MAP_BASE_NAME";
        public const string RULE_BASE_MAP_OWNER_NAME = "RULE_BASE_MAP_OWNER_NAME";
        public const string RULE_BASE_MAP_OWNER_ZONE = "RULE_BASE_MAP_OWNER_ZONE";
        public const string RULE_BASE_MAP_COMMENT = "RULE_BASE_MAP_COMMENT";
        public const string RULE_BASE_MAP_CREATE_TIME = "RULE_BASE_MAP_CREATE_TIME";
        public const string RULE_BASE_MAP_MODIFY_TIME = "RULE_BASE_MAP_MODIFY_TIME";
        public const string RULE_ID = "RULE_ID";
        public const string RULE_VERSION = "RULE_VERSION";
        public const string RULE_BASE_NAME = "RULE_BASE_NAME";
        public const string RULE_NAME = "RULE_NAME";
        public const string RULE_EVENT = "RULE_EVENT";
        public const string RULE_CONDITION = "RULE_CONDITION";
        public const string RULE_BODY = "RULE_BODY";
        public const string RULE_RECOVERY = "RULE_RECOVERY";
        public const string RULE_STATUS = "RULE_STATUS";
        public const string RULE_OWNER_NAME = "RULE_OWNER_NAME";
        public const string RULE_OWNER_ZONE = "RULE_OWNER_ZONE";
        public const string RULE_DESCR_1 = "RULE_DESCR_1";
        public const string RULE_DESCR_2 = "RULE_DESCR_2";
        public const string RULE_INPUT_PARAMS = "RULE_INPUT_PARAMS";
        public const string RULE_OUTPUT_PARAMS = "RULE_OUTPUT_PARAMS";
        public const string RULE_DOLLAR_VARS = "RULE_DOLLAR_VARS";
        public const string RULE_ICAT_ELEMENTS = "RULE_ICAT_ELEMENTS";
        public const string RULE_SIDEEFFECTS = "RULE_SIDEEFFECTS";
        public const string RULE_COMMENT = "RULE_COMMENT";
        public const string RULE_CREATE_TIME = "RULE_CREATE_TIME";
        public const string RULE_MODIFY_TIME = "RULE_MODIFY_TIME";
        public const string DVM_BASE_MAP_VERSION = "DVM_BASE_MAP_VERSION";
        public const string DVM_BASE_MAP_BASE_NAME = "DVM_BASE_MAP_BASE_NAME";
        public const string DVM_BASE_MAP_OWNER_NAME = "DVM_BASE_MAP_OWNER_NAME";
        public const string DVM_BASE_MAP_OWNER_ZONE = "DVM_BASE_MAP_OWNER_ZONE";
        public const string DVM_BASE_MAP_COMMENT = "DVM_BASE_MAP_COMMENT";
        public const string DVM_BASE_MAP_CREATE_TIME = "DVM_BASE_MAP_CREATE_TIME";
        public const string DVM_BASE_MAP_MODIFY_TIME = "DVM_BASE_MAP_MODIFY_TIME";
        public const string DVM_ID = "DVM_ID";
        public const string DVM_VERSION = "DVM_VERSION";
        public const string DVM_BASE_NAME = "DVM_BASE_NAME";
        public const string DVM_EXT_VAR_NAME = "DVM_EXT_VAR_NAME";
        public const string DVM_CONDITION = "DVM_CONDITION";
        public const string DVM_INT_MAP_PATH = "DVM_INT_MAP_PATH";
        public const string DVM_STATUS = "DVM_STATUS";
        public const string DVM_OWNER_NAME = "DVM_OWNER_NAME";
        public const string DVM_OWNER_ZONE = "DVM_OWNER_ZONE";
        public const string DVM_COMMENT = "DVM_COMMENT";
        public const string DVM_CREATE_TIME = "DVM_CREATE_TIME";
        public const string DVM_MODIFY_TIME = "DVM_MODIFY_TIME";
        public const string FNM_BASE_MAP_VERSION = "FNM_BASE_MAP_VERSION";
        public const string FNM_BASE_MAP_BASE_NAME = "FNM_BASE_MAP_BASE_NAME";
        public const string FNM_BASE_MAP_OWNER_NAME = "FNM_BASE_MAP_OWNER_NAME";
        public const string FNM_BASE_MAP_OWNER_ZONE = "FNM_BASE_MAP_OWNER_ZONE";
        public const string FNM_BASE_MAP_COMMENT = "FNM_BASE_MAP_COMMENT";
        public const string FNM_BASE_MAP_CREATE_TIME = "FNM_BASE_MAP_CREATE_TIME";
        public const string FNM_BASE_MAP_MODIFY_TIME = "FNM_BASE_MAP_MODIFY_TIME";
        public const string FNM_ID = "FNM_ID";
        public const string FNM_VERSION = "FNM_VERSION";
        public const string FNM_BASE_NAME = "FNM_BASE_NAME";
        public const string FNM_EXT_FUNC_NAME = "FNM_EXT_FUNC_NAME";
        public const string FNM_INT_FUNC_NAME = "FNM_INT_FUNC_NAME";
        public const string FNM_STATUS = "FNM_STATUS";
        public const string FNM_OWNER_NAME = "FNM_OWNER_NAME";
        public const string FNM_OWNER_ZONE = "FNM_OWNER_ZONE";
        public const string FNM_COMMENT = "FNM_COMMENT";
        public const string FNM_CREATE_TIME = "FNM_CREATE_TIME";
        public const string FNM_MODIFY_TIME = "FNM_MODIFY_TIME";
        public const string QUOTA_USER_ID = "QUOTA_USER_ID";
        public const string QUOTA_RESC_ID = "QUOTA_RESC_ID";
        public const string QUOTA_LIMIT = "QUOTA_LIMIT";
        public const string QUOTA_OVER = "QUOTA_OVER";
        public const string QUOTA_MODIFY_TIME = "QUOTA_MODIFY_TIME";
        public const string QUOTA_USAGE_USER_ID = "QUOTA_USAGE_USER_ID";
        public const string QUOTA_USAGE_RESC_ID = "QUOTA_USAGE_RESC_ID";
        public const string QUOTA_USAGE = "QUOTA_USAGE";
        public const string QUOTA_USAGE_MODIFY_TIME = "QUOTA_USAGE_MODIFY_TIME";
        public const string QUOTA_USER_NAME = "QUOTA_USER_NAME";
        public const string QUOTA_USER_ZONE = "QUOTA_USER_ZONE";
        public const string QUOTA_USER_TYPE = "QUOTA_USER_TYPE";
        public const string QUOTA_RESC_NAME = "QUOTA_RESC_NAME";
        public const string MSRVC_ID = "MSRVC_ID";
        public const string MSRVC_NAME = "MSRVC_NAME";
        public const string MSRVC_SIGNATURE = "MSRVC_SIGNATURE";
        public const string MSRVC_DOXYGEN = "MSRVC_DOXYGEN";
        public const string MSRVC_VARIATIONS = "MSRVC_VARIATIONS";
        public const string MSRVC_STATUS = "MSRVC_STATUS";
        public const string MSRVC_OWNER_NAME = "MSRVC_OWNER_NAME";
        public const string MSRVC_OWNER_ZONE = "MSRVC_OWNER_ZONE";
        public const string MSRVC_COMMENT = "MSRVC_COMMENT";
        public const string MSRVC_CREATE_TIME = "MSRVC_CREATE_TIME";
        public const string MSRVC_MODIFY_TIME = "MSRVC_MODIFY_TIME";
        public const string MSRVC_MODULE_NAME = "MSRVC_MODULE_NAME";
        public const string MSRVC_VERSION = "MSRVC_VERSION";
        public const string MSRVC_HOST = "MSRVC_HOST";
        public const string MSRVC_LOCATION = "MSRVC_LOCATION";
        public const string MSRVC_LANGUAGE = "MSRVC_LANGUAGE";
        public const string MSRVC_TYPE_NAME = "MSRVC_TYPE_NAME";
        public const string MSRVC_VER_OWNER_NAME = "MSRVC_VER_OWNER_NAME";
        public const string MSRVC_VER_OWNER_ZONE = "MSRVC_VER_OWNER_ZONE";
        public const string MSRVC_VER_COMMENT = "MSRVC_VER_COMMENT";
        public const string MSRVC_VER_CREATE_TIME = "MSRVC_VER_CREATE_TIME";
        public const string MSRVC_VER_MODIFY_TIME = "MSRVC_VER_MODIFY_TIME";
        public const string META_ACCESS_TYPE = "META_ACCESS_TYPE";
        public const string META_ACCESS_NAME = "META_ACCESS_NAME";
        public const string META_TOKEN_NAMESPACE = "META_TOKEN_NAMESPACE";
        public const string META_ACCESS_USER_ID = "META_ACCESS_USER_ID";
        public const string META_ACCESS_META_ID = "META_ACCESS_META_ID";
        public const string RESC_ACCESS_TYPE = "RESC_ACCESS_TYPE";
        public const string RESC_ACCESS_NAME = "RESC_ACCESS_NAME";
        public const string RESC_TOKEN_NAMESPACE = "RESC_TOKEN_NAMESPACE";
        public const string RESC_ACCESS_USER_ID = "RESC_ACCESS_USER_ID";
        public const string RESC_ACCESS_RESC_ID = "RESC_ACCESS_RESC_ID";
        public const string RULE_ACCESS_TYPE = "RULE_ACCESS_TYPE";
        public const string RULE_ACCESS_NAME = "RULE_ACCESS_NAME";
        public const string RULE_TOKEN_NAMESPACE = "RULE_TOKEN_NAMESPACE";
        public const string RULE_ACCESS_USER_ID = "RULE_ACCESS_USER_ID";
        public const string RULE_ACCESS_RULE_ID = "RULE_ACCESS_RULE_ID";
        public const string MSRVC_ACCESS_TYPE = "MSRVC_ACCESS_TYPE";
        public const string MSRVC_ACCESS_NAME = "MSRVC_ACCESS_NAME";
        public const string MSRVC_TOKEN_NAMESPACE = "MSRVC_TOKEN_NAMESPACE";
        public const string MSRVC_ACCESS_USER_ID = "MSRVC_ACCESS_USER_ID";
        public const string MSRVC_ACCESS_MSRVC_ID = "MSRVC_ACCESS_MSRVC_ID";
        public const string TICKET_ID = "TICKET_ID";
        public const string TICKET_STRING = "TICKET_STRING";
        public const string TICKET_TYPE = "TICKET_TYPE";
        public const string TICKET_USER_ID = "TICKET_USER_ID";
        public const string TICKET_OBJECT_ID = "TICKET_OBJECT_ID";
        public const string TICKET_OBJECT_TYPE = "TICKET_OBJECT_TYPE";
        public const string TICKET_USES_LIMIT = "TICKET_USES_LIMIT";
        public const string TICKET_USES_COUNT = "TICKET_USES_COUNT";
        public const string TICKET_WRITE_FILE_COUNT = "TICKET_WRITE_FILE_COUNT";
        public const string TICKET_WRITE_FILE_LIMIT = "TICKET_WRITE_FILE_LIMIT";
        public const string TICKET_WRITE_BYTE_COUNT = "TICKET_WRITE_BYTE_COUNT";
        public const string TICKET_WRITE_BYTE_LIMIT = "TICKET_WRITE_BYTE_LIMIT";
        public const string TICKET_EXPIRY = "TICKET_EXPIRY";
        public const string TICKET_CREATE_TIME = "TICKET_CREATE_TIME";
        public const string TICKET_MODIFY_TIME = "TICKET_MODIFY_TIME";
        public const string TICKET_ALLOWED_HOST_TICKET_ID = "TICKET_ALLOWED_HOST_TICKET_ID";
        public const string TICKET_ALLOWED_HOST = "TICKET_ALLOWED_HOST";
        public const string TICKET_ALLOWED_USER_TICKET_ID = "TICKET_ALLOWED_USER_TICKET_ID";
        public const string TICKET_ALLOWED_USER_NAME = "TICKET_ALLOWED_USER_NAME";
        public const string TICKET_ALLOWED_GROUP_TICKET_ID = "TICKET_ALLOWED_GROUP_TICKET_ID";
        public const string TICKET_ALLOWED_GROUP_NAME = "TICKET_ALLOWED_GROUP_NAME";
        public const string TICKET_DATA_NAME = "TICKET_DATA_NAME";
        public const string TICKET_DATA_COLL_NAME = "TICKET_DATA_COLL_NAME";
        public const string TICKET_COLL_NAME = "TICKET_COLL_NAME";
        public const string TICKET_OWNER_NAME = "TICKET_OWNER_NAME";
        public const string TICKET_OWNER_ZONE = "TICKET_OWNER_ZONE";

    }
}
